using Lsn1.Model;
using Microsoft.AspNetCore.Mvc;

namespace Lsn1.Controllers;

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
    

    // [Route("/api/[controller]")]
    // public IActionResult Index()
    // {
    //     return View();
    // }
}