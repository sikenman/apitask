using Microsoft.AspNetCore.Mvc;
using WebApi.Model;
using WebApi.Service;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly InMemoryProductService _productService = new();

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _productService.GetProducts();
            return Ok(products);
        }
        
        [HttpGet("search")]
        public IActionResult SearchProducts([FromQuery] string name, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] DateTime? postedDateStart, [FromQuery] DateTime? postedDateEnd)
        {
            var products = _productService.SearchProducts(name, minPrice, maxPrice, postedDateStart, postedDateEnd);
            return Ok(products);
        }

        [HttpGet("queue")]
        public IActionResult GetProductsInApprovalQueue()
        {
            var products = _productService.GetProductsInApprovalQueue();
            return Ok(products);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            try
            {
                _productService.CreateProduct(product);
                return Ok("Product created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            try
            {
                _productService.UpdateProduct(id, product);
                return Ok("Product updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return Ok("Product delete request submitted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("approve/{id}")]
        public IActionResult ApproveProduct(int id)
        {
            try
            {
                _productService.ApproveProduct(id);
                return Ok("Product approved successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reject/{id}")]
        public IActionResult RejectProduct(int id)
        {
            try
            {
                _productService.RejectProduct(id);
                return Ok("Product rejected successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
