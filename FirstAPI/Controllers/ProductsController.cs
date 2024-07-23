using FirstAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        
        private readonly ShopContext _shopContext;

        public ProductsController(ShopContext shopContext) { 
            _shopContext = shopContext;

            _shopContext.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            return Ok(await _shopContext.Products.ToArrayAsync()) ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]

        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();

/*          For CUstom Error Handling : 
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }*/

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            _shopContext.Entry(product).State = EntityState.Modified;
            try
            {
                await _shopContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!_shopContext.Products.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
                
            }
            return NoContent();

        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<Product>> DeleteProduct(int id) {
            var deletedProduct = await _shopContext.Products.FindAsync(id);
            if (deletedProduct == null)
            {
                return NotFound();
            }
            _shopContext.Products.Remove(deletedProduct);
            await _shopContext.SaveChangesAsync();
            return deletedProduct;
        }


        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<Product>> DeleteMultipleProducts([FromQuery]int[] ids) {

            var products = new List<Product>();
            foreach (var id in ids) {
                var product = await _shopContext.Products.FindAsync(id);
                if(product == null)
                {
                    return NotFound();
                }
                products.Add(product);
            }
            
            _shopContext.Products.RemoveRange(products);
            await _shopContext.SaveChangesAsync();
            return Ok(products);
        }

        



    }
}
