using IDistrubutedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IDistrubutedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {

        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;


        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions distributedCacheEntryOptions = new
            DistributedCacheEntryOptions();
            distributedCacheEntryOptions.AbsoluteExpiration = System.DateTime.Now.AddMinutes(2);
            _distributedCache.SetString("Name", "Berkay", distributedCacheEntryOptions);
            await _distributedCache.SetStringAsync("NameAsync", "Beko",distributedCacheEntryOptions);

            //Complex Types caching
            Product product1 = new Product
            {
                Id = 457812,
                Name = "Acer Aspire3 16GB",
                Price = 16000
            };
            string jsonProduct=JsonConvert.SerializeObject(product1);

       

            await _distributedCache.SetStringAsync("product:1", jsonProduct,distributedCacheEntryOptions);
            return View();
        }

        public IActionResult Show()
        {
            ViewBag.name = _distributedCache.GetString("Name");
            var product = _distributedCache.GetString("product:1");
            ViewBag.product=JsonConvert.DeserializeObject<Product>(product);

            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("Name");
            return View();
        }
    }
}
