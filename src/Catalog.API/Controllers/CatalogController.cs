using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }

        [Route("[action]/{name}", Name = "GetProductsByName")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            var products = await _productRepository.GetProductByName(name);
            return products;
        }

        [Route("[action]/{category}", Name ="GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var products = await _productRepository.GetProductByCategory(category);
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name ="GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product==null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
           await _productRepository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product); 
        }


        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult>  DeleteProduct(string id)
        {
            var deleteresult = await _productRepository.DeleteProduct(id);
            return Ok(deleteresult);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var updateResult = await _productRepository.UpdateProduct(product);
            return Ok(updateResult);
        }
    }
}
