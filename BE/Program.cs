
using System.IO.Compression;
using System.Net;
using DemoImportExport.Caches;
using DemoImportExport.Mapping;
using DemoImportExport.Models.Response;
using DemoImportExport.Persistents;
using DemoImportExport.Repositories.DepartmentRepositories;
using DemoImportExport.Repositories.EmployeeRepositories;
using DemoImportExport.Repositories.PositionRepositories;
using DemoImportExport.Services.DepartmentServices;
using DemoImportExport.Services.EmployeeServices;
using DemoImportExport.Services.PositionServices;
using DemoImportExport.Uow;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

namespace DemoImportExport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Early init of NLog to allow startup and exception logging, before host is built
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                });

                builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });

                builder.Services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.SmallestSize;
                });
                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));
                // Repository
                builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
                // Services
                builder.Services.AddScoped<ICacheService, CacheService>();

                builder.Services.AddScoped<IEmployeeService, EmployeeService>();
                builder.Services.AddScoped<IDepartmentService, DepartmentService>();
                builder.Services.AddScoped<IPositionService, PositionService>();
                builder.Services.AddScoped<ICacheService, CacheService>();
                builder.Services.AddAutoMapper(typeof(EmployeeProfile));

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                var app = builder.Build();
                app.UseResponseCompression();
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                    app.UseHsts();
                }

                // use scope ensure connect to db 
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbContext.Database.Migrate();
                }

                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            var response = new ApiResponse<object>
                            {
                                Status = 500,
                                Message = error.Error.Message
                            };

                            await context.Response.WriteAsJsonAsync(response);
                        }
                    });
                });

                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();

                app.UseAuthorization();

                app.Use(async (context, next) =>
                {
                    await next();

                    if (context.Response.StatusCode == (int)HttpStatusCode.NotFound && context.GetEndpoint() == null)
                    {
                        await context.Response.WriteAsJsonAsync(new ApiResponse<object>()
                        {
                            Status = (int)HttpStatusCode.NotFound,
                            Message = "Not Found",
                        });
                    }
                });

                app.MapControllers();

                app.Run();
            }
            catch (Exception exception)
            {
                // NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }
}
