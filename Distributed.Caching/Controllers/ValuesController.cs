using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;

        public string _CacheKey = "";
        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet(Name = "SetValue")]
        public async Task<IActionResult> Set(string menuName, string contactNo)
        {
            await _distributedCache.SetStringAsync(CacheKeys.Menu, menuName, options: new()
            {
                AbsoluteExpiration = DateTime.UtcNow.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(10)
            });
            await _distributedCache.SetAsync(CacheKeys.Contact, Encoding.UTF8.GetBytes(contactNo), options: new()
            {
                AbsoluteExpiration = DateTime.UtcNow.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(10)
                //10 sn içinde işlem yapılmadığı anda cache temizlenir.
            });

            return Ok();
        }

        [HttpGet(Name = "GetValue")]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetStringAsync(CacheKeys.Menu);
            var noBinary = await _distributedCache.GetAsync(CacheKeys.Contact);
            var nostr = Encoding.UTF8.GetString(noBinary);

            string menuText = "";
            if (name != null)
            {
                menuText = name;
            }
            else
            {
                menuText = "Cache silindi..";
            }

            return Ok(new { menuText, nostr });
        }
    }
}
