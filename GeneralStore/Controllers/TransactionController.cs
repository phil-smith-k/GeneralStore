using GeneralStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStore.Controllers
{
    public class TransactionController : ApiController
    {
        private readonly ApplicationDbContext _dbContext = ApplicationDbContext.Create();

        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] Transaction transaction)
        {
            if (transaction == null)
                return BadRequest();

            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);

            // Lookup both the customer and product using the _dbContext
            var customer = await _dbContext.Customers.FindAsync(transaction.CustomerId);
            var product = await _dbContext.Products.FindAsync(transaction.Sku);

            var validationResult = this.ValidateTransaction(transaction, product, customer);

            if (!string.IsNullOrWhiteSpace(validationResult))
                return BadRequest(validationResult);

            product.NumberInInventory -= transaction.ItemCount;
            _dbContext.Transactions.Add(transaction);

            if (await _dbContext.SaveChangesAsync() > 0)
                return Ok(transaction);

            return InternalServerError();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll(int skip = 0, int take = 5)
        {
            var results = _dbContext
                .Transactions
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take);

            return Ok(results);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            var transaction = await _dbContext.Products.FindAsync(id);

            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        private string ValidateTransaction(Transaction transaction, Product product, Customer customer)
        {
            if (customer == null)
                return $"Invalid transaction: Customer with Id: {transaction.CustomerId} does not exist.";

            if (product == null)
                return $"Invalid Transaction: Product with Sku: {transaction.Sku} does not exist.";

            if (!product.IsInStock)
                return $"Invalid transaction: Product with Sku: {transaction.Sku} is not in stock";

            if (product.NumberInInventory < transaction.ItemCount)
                return $"Invalid transaction: Product with Sku: {transaction.Sku} does not have enough on-hand to complete transaction.";

            return string.Empty;
        }
    }
}
