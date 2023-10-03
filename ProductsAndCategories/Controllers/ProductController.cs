#pragma warning disable CS8602
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private MyContext _context;

    public ProductController(ILogger<ProductController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    //! Root -> Redirect to All Products
    [HttpGet("")]
    public RedirectResult Index()
    {
        return Redirect("/products");
    }

    //! View all Products
    [HttpGet("products")]
    public ViewResult AllProducts()
    {
        // Get all Products alphabetically
        // ViewBag.AllProducts = _context.Products.OrderBy(p => p.Name).ToList(); 
        List<Product> AllProducts =  _context.Products.OrderBy(p => p.Name).ToList();      
        return View(AllProducts);
    }

    //! Create a new Product
    [HttpPost("products/create")]
    public IActionResult CreateProduct(Product newProduct)
    {
        if(!ModelState.IsValid)
        {
            return View("Index");
        }

        _context.Add(newProduct);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    //! Show a Product
    [HttpGet("products/{id}")]
    public ViewResult ShowProduct(int id)
    {
        // Get One Product by ProductId -> Include Categories Association -> Then Include Associated Categories -> Order Alphabetically
        Product? OneProduct = _context.Products.Include(p => p.Categories).ThenInclude(c => c.Category).OrderBy(c => c.Name).FirstOrDefault(p => p.ProductId == id);
        // Get all Categories NOT associated with the OneProduct
        ViewBag.AllCategories = _context.Categories.Include(c => c.Products).Where(c => c.Products.All(pa => pa.ProductId != id)).OrderBy(c => c.Name);
        
        return View(OneProduct);
    }
    
    //! Errors
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
