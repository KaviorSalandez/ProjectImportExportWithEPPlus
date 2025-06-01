namespace DemoImportExport.Caches
{
    public interface ICacheService
    {
        T GetData<T>(string key);

        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);

        Object Delete(string key);
    }
}
