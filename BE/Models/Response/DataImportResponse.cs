namespace DemoImportExport.Models.Response
{
    public class DataImportResponse
    {
        public string KeyRedis { get; set; }
        public object DataExists { get; set; }
        public object DataImport { get; set; }
        public string FileUrl { get; set; }    // Url file lưu trong redis cho fe tải xuống
    }
}
