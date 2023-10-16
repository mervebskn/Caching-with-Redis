using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Redis.Sentinel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DbContextClass _dbContext;
        private readonly ICacheService _cacheService;
        public ProductController(DbContextClass dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }
        [HttpGet("products")]
        public async Task<List<Product>> Get()
        {
            var cacheData = await _cacheService.GetData<List<Product>>("product");
            if (cacheData != null)
            {
                return cacheData;
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = _dbContext.Products.ToList();
            _cacheService.SetData<List<Product>>("product", cacheData, expirationTime);
            return cacheData;
        }
        [HttpGet("product")]
        public async Task<Product> Get(int id)
        {
            Product filteredData;
            var cacheData =await _cacheService.GetData<List<Product>>("product");
            if (cacheData != null)
            {
                filteredData = cacheData.Where(x => x.ProductId == id).FirstOrDefault();
                return filteredData;
            }
            filteredData =await _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
            return filteredData;
        }
        [HttpPost("addProduct")]
        public async Task<Product> Post(Product value)
        {
            var _object = await _dbContext.Products.AddAsync(value);
            _cacheService.RemoveData("product");
            await _dbContext.SaveChangesAsync();
            return _object.Entity;
        }
        [HttpPut("updateProduct")]
        public void Put(Product product)
        {
            _dbContext.Products.Update(product);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
        }
        [HttpDelete("deleteProduct")]
        public void Delete(int Id)
        {
            var filteredData = _dbContext.Products.Where(x => x.ProductId == Id).FirstOrDefault();
            _dbContext.Remove(filteredData);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
        }
    }
}
