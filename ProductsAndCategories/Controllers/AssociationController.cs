using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers;

public class AssociationController : Controller
{
    private readonly ILogger<AssociationController> _logger;
    private MyContext _context;

    public AssociationController(ILogger<AssociationController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    //! Add Category to Product
    [HttpPost("associations/create/products/{id}")]
    public IActionResult CreateCategoryAssociation(int id, int addedCategory)
    {
        Association NewAssociation = new()
        {
            ProductId = id,
            CategoryId = addedCategory
        };

        _context.Add(NewAssociation);
        _context.SaveChanges();
        
        return Redirect($"/products/{id}");
    }

    //! Add Product to Category
    [HttpPost("associations/create/categories/{id}")]
    public IActionResult CreateProductAssociation(int id, int addedProduct)
    {
        Association NewAssociation = new()
        {
            ProductId = addedProduct,
            CategoryId = id
        };

        _context.Add(NewAssociation);
        _context.SaveChanges();

        return Redirect($"/categories/{id}");
    }

    //! Delete an Association
    [HttpPost("associations/delete/{id}/{path}/{pathId}")]
    public IActionResult DeleteAssociation(int id, string path, int pathId)
    {
        Association? AssociationToDelete = _context.Associations.SingleOrDefault(a => a.AssociationId == id);
        _context.Associations.Remove(AssociationToDelete);
        _context.SaveChanges();

        // Redirect back to the route you were on
        return Redirect($"/{path}/{pathId}");
    }
    
    //! Errors
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
