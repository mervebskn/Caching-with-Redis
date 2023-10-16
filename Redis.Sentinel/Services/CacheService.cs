using Newtonsoft.Json;
using StackExchange.Redis;

namespace Redis.Sentinel
{
    public class CacheService : ICacheService
    {
        RedisService redisService;
        IDatabase _db;
        public CacheService()
        {
            ConfigureRedis();
        }
        private async Task ConfigureRedis()
        {
            _db = await redisService.RedisMasterDatabase();
        }
        public async Task<T> GetData<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }
        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = await _db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool keyExist = _db.KeyExists(key);
            if (keyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }
        public bool IsSet(string key)
        {
            return _db.KeyExists(key) != null;
        }

    }
}
