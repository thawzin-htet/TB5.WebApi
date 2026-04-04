using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TB5.WebApi.Database.AppDbContextModels;
using TB5.WebApi.Models;

namespace TB5.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext db = new AppDbContext();

        // GET: api/Product
        // Response: 200 OK
        [HttpGet]
        public IActionResult GetProducts()
        {
            var lst = db.TblProducts.Where(x => !x.IsDelete).ToList();
            return Ok(lst);
        }

        // GET: api/Product/1
        // Response: 200 OK or 404 Not Found
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var itemProduct = db.TblProducts.FirstOrDefault(x => x.Id == id && !x.IsDelete);
            if (itemProduct is null)
            {
                return NotFound(new { Message = "Product not found." });
            }
            return Ok(itemProduct);
        }

        // POST: api/Product
        // Request Body: { "name": "New Product", "price": 99.99 }
        // Response: 201 Created
        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = new TblProduct
            {
                Name = request.Name,
                Price = request.Price,
                CreatedDateTime = DateTime.Now,
                IsDelete = false
            };

            db.TblProducts.Add(product);
            db.SaveChanges();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/Product/1
        // Request Body: { "name": "Updated Product", "price": 120.00 }
        // Response: 200 OK or 404 Not Found
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductUpdateDto request)
        {
            var itemProduct = db.TblProducts.FirstOrDefault(x => x.Id == id && !x.IsDelete);
            if (itemProduct is null)
            {
                return NotFound(new { Message = "Product not found." });
            }

            itemProduct.Name = request.Name;
            itemProduct.Price = request.Price;

            db.SaveChanges();
            return Ok(new { Message = "Product updated successfully.", Data = itemProduct });
        }

        // PATCH: api/Product/1
        // Request Body: [ { "op": "replace", "path": "/name", "value": "Patched Name" } ]
        // Response: 200 OK or 404 Not Found
        [HttpPatch("{id}")]
        public IActionResult PatchProduct(int id, [FromBody] JsonPatchDocument<TblProduct> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            var itemProduct = db.TblProducts.FirstOrDefault(x => x.Id == id && !x.IsDelete);
            if (itemProduct is null)
            {
                return NotFound(new { Message = "Product not found." });
            }

            patchDoc.ApplyTo(itemProduct, ModelState);

            if (!TryValidateModel(itemProduct))
            {
                return BadRequest(ModelState);
            }

            db.SaveChanges();
            return Ok(new { Message = "Product patched successfully.", Data = itemProduct });
        }

        // DELETE: api/Product/1
        // Response: 200 OK or 404 Not Found
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var itemProduct = db.TblProducts.FirstOrDefault(x => x.Id == id && !x.IsDelete);
            if (itemProduct is null)
            {
                return NotFound(new { Message = "Product not found." });
            }

            // Soft delete
            itemProduct.IsDelete = true;
            db.SaveChanges();

            return Ok(new { Message = "Product deleted successfully." });
        }
    }

    public class ProductUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}