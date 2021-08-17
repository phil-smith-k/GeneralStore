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
    public class CustomerController : ApiController
    {
        private readonly ApplicationDbContext _dbContext = ApplicationDbContext.Create();

        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] Customer customer)
        {
            // Validating: is the data that's coming in, in the correct format? 
            if (customer == null)
                return BadRequest();

            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);

            _dbContext.Customers.Add(customer);

            if (await _dbContext.SaveChangesAsync() > 0)
                return Ok(customer);

            return InternalServerError();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll(int skip = 0, int take = 5)
        {
            var results =_dbContext
                .Customers
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take);

            return Ok(results);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest();

            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);

            var existing = await _dbContext.Customers.FindAsync(customer.Id);

            if (existing == null)
                return NotFound();

            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;

            if (await _dbContext.SaveChangesAsync() > 0)
                return Ok(existing);

            return InternalServerError();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete([FromUri] int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            _dbContext.Customers.Remove(customer);

            if (await _dbContext.SaveChangesAsync() > 0)
                return Ok();

            return InternalServerError();
        }
    }
}
