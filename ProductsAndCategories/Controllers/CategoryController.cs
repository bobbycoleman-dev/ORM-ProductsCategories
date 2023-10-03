#pragma warning disable CS8602
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers;

public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private MyContext _context;

    public CategoryController(ILogger<CategoryController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    //! View All Categories
    [HttpGet("categories")]
    public IActionResult AllCategories()
    {
        // Get all Categories alphabetically
        ViewBag.AllCategories = _context.Categories.OrderBy(c => c.Name).ToList();
        Console.WriteLine();
        
        return View();
    }

    //! Create a new Category
    [HttpPost("categories/create")]
    public IActionResult CreateCategory(Category newCategory)
    {
        if(!ModelState.IsValid)
        {
            return View("AllCategories");
        }

        _context.Add(newCategory);
        _context.SaveChanges();
        return RedirectToAction("AllCategories");
    }

    //! Show a Category
    [HttpGet("categories/{id}")]
    public ViewResult ShowCategory(int id)
    {
        // Get One Category by CategoryId -> Include Product Association -> Then Include Associated Products -> Order Alphabetically
        Category? OneCategory = _context.Categories.Include(c => c.Products).ThenInclude(p => p.Product).OrderBy(p => p.Name).FirstOrDefault(c => c.CategoryId == id);
        // Select only Name from OneCategory associated products list
        List<string> OneCategoryProducts = OneCategory.Products.Select(a => a.Product.Name).ToList();
        // Get all Categories NOT associated with the OneCategory
        ViewBag.AllProducts = _context.Products.OrderBy(p => p.Name).Where(p => !OneCategoryProducts.Contains(p.Name));
        
        return View(OneCategory);
    }

    //! Errors
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
