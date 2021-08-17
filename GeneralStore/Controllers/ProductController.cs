using GeneralStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStore.Controllers
{
    public class ProductController : ApiController
    {
        private readonly ApplicationDbContext _dbContext = ApplicationDbContext.Create();

        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);

            _dbContext.Products.Add(product);

            if (await _dbContext.SaveChangesAsync() > 0)
                return Ok(product);

            return InternalServerError();
        }

        [HttpGet]
        [Route("api/Product/{sku}")]
        public async Task<IHttpActionResult> GetBySku(string sku)
        {
            var products = await _dbContext.Products.FindAsync(sku);

            if (products == null)
                return NotFound();

            return Ok(products);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll(int skip = 0, int take = 5)
        {
            var results = _dbContext
                .Products
                .OrderBy(x => x.Sku)
                .Skip(skip)
                .Take(take);

            return Ok(results);
        }
    }
}
