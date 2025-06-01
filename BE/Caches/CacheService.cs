using StackExchange.Redis;
using System.Text.Json;

namespace DemoImportExport.Caches
{
    public class CacheService : ICacheService
    {
        public IDatabase _cacheDb;
        public CacheService()
        {

            var redis = ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false,connectTimeout=30000,responseTimeout=30000");
            if (redis != null)
            {

                _cacheDb = redis.GetDatabase(); // Kết nối tới database số 0
            }
            else
            {
                throw new Exception("Không thể kết nối đến Redis cache. Vui lòng kiểm tra cài đặt và đảm bảo rằng Redis đang hoạt động.");
            }

        }
        public object Delete(string key)
        {
            var _exist = _cacheDb.KeyExists(key);
            if (_exist)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expirtyTime = expirationTime.UtcDateTime.Subtract(DateTime.UtcNow);
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expirtyTime);
        }
    }
}
