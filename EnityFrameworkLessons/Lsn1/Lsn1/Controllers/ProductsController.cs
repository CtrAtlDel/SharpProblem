using System.Reflection.Metadata.Ecma335;
using Lsn1.Model;
using Microsoft.AspNetCore.Mvc;

namespace Lsn1.Controllers;

[Route("/api/[controller]")]
public class ProductsController : Controller
{
    private static List<Product> products = new List<Product>(new[]
    {
        new Product() {Id = 1, Name = "Notebook", Price = 100_000},
        new Product() {Id = 2, Name = "Car", Price = 2_000_000},
        new Product() {Id = 3, Name = "Apple", Price = 30}
    });

    [HttpGet]
    public IEnumerable<Product> Get() => products;

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var product =
            products.SingleOrDefault(p => p.Id == id); // бежит по товару, проверяет предикат, соответствует ли id

        if (product == null)
        {
            return NotFound(); // ERROR 404
        }

        return Ok(product);
    }

    //Delete method
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        products.Remove(products.SingleOrDefault(p => p.Id == id));
        return Ok(new {message = "Deleted successfully"});
    }


    // Id геенерация
    private int NextProductid => products.Count() == 0 ? 1 : products.Max(x => x.Id) + 1;

    [HttpGet("GetNextProductId")] // проверка будет: /api/GetNextProductId/
    public int GetNextProductId()
    {
        return NextProductid;
    }

    //Create or delete
    [HttpPost]
    public IActionResult Post(Product product)
    {
        // Валидация товара согласно модели
        if (!ModelState.IsValid) // не прошли проверу, [required]  не зря помечали ))
        {
            return BadRequest();
        }

        product.Id = NextProductid;
        products.Add(product);
        return CreatedAtAction(nameof(Get), new {id = product.Id}, product);
    }

    [HttpPost("AddProduct")]
    public IActionResult PostBody([FromBody] Product product) => Post(product);

    
    //Update
    [HttpPut]
    public IActionResult Put(Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var storedProduct = products.SingleOrDefault(p => p.Id == product.Id);
        if (storedProduct == null) 
            return NotFound();
        storedProduct.Name = product.Name;
        storedProduct.Price = product.Price;
        
        return Ok(storedProduct);
    }

    [HttpPut("UpdateProduct")]
    public IActionResult PutBody([FromBody] Product product) => Put(product);

    // [Route("/api/[controller]")]
    // public IActionResult Index()
    // {
    //     return View();
    // }
}