using DemoImportExport.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DemoImportExport.Persistents
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Department>()
                .Property(d => d.DepartmentId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Position>()
                .Property(p => p.PositionId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "Phòng Kỹ thuật phần mềm" },
                new Department { DepartmentId = 2, DepartmentName = "Phòng Nhân sự" },
                new Department { DepartmentId = 3, DepartmentName = "Phòng Kế toán" }
            );

            modelBuilder.Entity<Position>().HasData(
                new Position { PositionId = 1, PositionName = "Lập trình viên" },
                new Position { PositionId = 2, PositionName = "Trưởng phòng" },
                new Position { PositionId = 3, PositionName = "Kế toán viên" }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    EmployeeName = "Nguyễn Văn A",
                    DepartmentId = 1,
                    PositionId = 1,
                    EmployeeCode = "NV001",
                    DOB = new DateTime(1990, 5, 20),
                    Gender = DemoImportExport.Enums.CDKEnum.Gender.Nam,
                    IDNo = "123456789",
                    IssueDate = new DateTime(2010, 1, 1),
                    IssuedBy = "Công an Hà Nội",
                    Address = "Hà Nội",
                    MobilePhone = "0912345678",
                    LandlinePhone = "0241234567",
                    Email = "nva@example.com",
                    BankAccount = "123456789",
                    BankName = "Vietcombank",
                    Branch = "Hà Nội",
                    Password = "123@abc",
                    TotalRecord = 10
                },
                new Employee
                {
                    EmployeeId = 2,
                    EmployeeName = "Trần Thị B",
                    DepartmentId = 2,
                    PositionId = 2,
                    EmployeeCode = "NV002",
                    DOB = new DateTime(1985, 3, 15),
                    Gender = DemoImportExport.Enums.CDKEnum.Gender.Nữ,
                    IDNo = "987654321",
                    IssueDate = new DateTime(2005, 6, 1),
                    IssuedBy = "Công an TP HCM",
                    Address = "TP HCM",
                    MobilePhone = "0909123456",
                    LandlinePhone = "0281234567",
                    Email = "ttb@example.com",
                    BankAccount = "987654321",
                    BankName = "ACB",
                    Branch = "TP HCM",
                    Password = "456@def",
                    TotalRecord = 12
                },
                new Employee
                {
                    EmployeeId = 3,
                    EmployeeName = "Lê Văn C",
                    DepartmentId = 3,
                    PositionId = 3,
                    EmployeeCode = "NV003",
                    DOB = new DateTime(1992, 11, 10),
                    Gender = DemoImportExport.Enums.CDKEnum.Gender.Nam,
                    IDNo = "111222333",
                    IssueDate = new DateTime(2012, 9, 15),
                    IssuedBy = "Công an Đà Nẵng",
                    Address = "Đà Nẵng",
                    MobilePhone = "0938123456",
                    LandlinePhone = "0236123456",
                    Email = "lvc@example.com",
                    BankAccount = "111222333",
                    BankName = "BIDV",
                    Branch = "Đà Nẵng",
                    Password = "789@ghi",
                    TotalRecord = 8
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
