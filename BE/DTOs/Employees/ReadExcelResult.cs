namespace DemoImportExport.DTOs.Employees
{
    public class ReadExcelResult<TDto>
    {
        public List<TDto> AllData { get; set; }
        public List<TDto> DataExists { get; set; }
        public List<TDto> DataImport { get; set; }
    }
}
