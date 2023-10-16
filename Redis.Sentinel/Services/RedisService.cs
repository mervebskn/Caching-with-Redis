using StackExchange.Redis;

namespace Redis.Sentinel
{
    public class RedisService
    {
        static ConfigurationOptions sentineloptions => new()
        {
            //redis sunucu bilgileri
            EndPoints = { { "localhost", 6382 }, { "localhost", 6383 } },
            CommandMap= CommandMap.Sentinel,
            AbortOnConnectFail = false,
        };
        static ConfigurationOptions masteroptions => new()
        {
            AbortOnConnectFail = false,
        };
        public async Task<IDatabase> RedisMasterDatabase()
        {
            ConnectionMultiplexer sentinelconn = await ConnectionMultiplexer.SentinelConnectAsync(sentineloptions);
            System.Net.EndPoint masterEndPoint = null;
            foreach (System.Net.EndPoint endp in sentinelconn.GetEndPoints())
            {
                IServer server = sentinelconn.GetServer(endp);
                if (!server.IsConnected)
                    continue;
                masterEndPoint = await server.SentinelGetMasterAddressByNameAsync("masterserver");
                break;
            }
            var localMasterIP = masterEndPoint.ToString() switch
            {
                "127.0.0.1:6379" => "localhost:6379",
                "127.0.0.2:6379" => "localhost:6380",
                "127.0.0.3:6379" => "localhost:6381"
            };
            ConnectionMultiplexer masterConn = await ConnectionMultiplexer.ConnectAsync(localMasterIP);
            IDatabase db = masterConn.GetDatabase();
            return db;
        }
    }
}
