using DemoImportExport.DTOs.Employees;
using DemoImportExport.Models;
using DemoImportExport.Models.Response;

namespace DemoImportExport.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);

        public Task<EmployeeCountDto> FindAllFilter(int pageSize = 10, int pageNumber = 1, string search = "", string? email = "");

        /// <summary>
        /// Tên hàm: export excel bảng nhân viên  
        /// </summary>
        /// <param name="data">Dữ liệu muốn export </param>
        /// <returns></returns>
        public Task<byte[]> ExportExcel(bool isFileMau, List<int>? Ids = null);

        public Task<byte[]> ExportExcel2(bool isFileMau, string? keyRedis);

        /// <summary>
        /// Tên hàm : generate mã code cho thực thể 
        /// </summary>
        /// <returns>Mã code được generate</returns>
        public Task<string> GenerateCode();

        /// <summary>
        /// Tên hàm: Import nhân viên trong  file excel 
        /// </summary>
        /// <param name="formfile">truyền vào một file excel</param>
        /// <returns>true: danh sách các lỗi hoặc thành công của mỗi bản ghi khi thêm </returns>
        ///  created by: Cấn Duy Khánh
        ///  created_at: 29/05/2025

        public Task<EmployeeImportParentDto> ImportExcel(IFormFile formFile);

        /// <summary>
        /// Tên hàm: Import nhân viên trong và database
        /// </summary>
        /// <param name="idCache">phần tử valid được lưu trong cache </param>
        /// <returns>true: số bản ghi được create  </returns>
        ///  created by: Đặng Đình Quốc Khánh 
        ///  created_at: 2023/12/20 
        public int ImportDatabase(string idImport);
        public Task<DataImportResponse> HandleDataImport(IFormFile file);
    }
}
