namespace Redis.Sentinel
{
    public interface ICacheService
    {
        Task<T> GetData<T>(string key);
        Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime);
        bool IsSet(string key);
        object RemoveData(string key);
    }
}
