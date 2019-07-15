using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ProductsAndCategories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        // Display Index Page
        [HttpGet("")]
        public ViewResult Index()
        {
            return View("Index");
        }

        // Display Products Page
        [HttpGet("products")]
        public ViewResult Products()
        {
            List<Product> AllProducts = dbContext.Products
            .Include(p => p.Associations)
            .ThenInclude(cat => cat.AssociationCategory)
            .ToList();
            ViewBag.AllProducts = AllProducts;
            return View("Product");
        }

        // Display Categories Page
        [HttpGet("categories")]
        public ViewResult Categories()
        {
            List<Category> AllCategories = dbContext.Categories
            .Include(c => c.Associations)
            .ThenInclude(p => p.AssociationProduct)
            .ToList();
            ViewBag.AllCategories = AllCategories;
            return View("Category");
        }

        // Post route for creating new products
        [HttpPost("create/product")]
        public IActionResult CreateProduct(Product newproduct)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newproduct);
                dbContext.SaveChanges();
                return RedirectToAction("Products");
            }
            else
            {
                List<Product> AllProducts = dbContext.Products
                .Include(p => p.Associations)
                .ThenInclude(cat => cat.AssociationCategory)
                .ToList();
                ViewBag.AllProducts = AllProducts;
                return View("Product");
            }
        }

        // Post route for creating new categories
        [HttpPost("create/category")]
        public IActionResult CreateCategory(Category newCategory)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newCategory);
                dbContext.SaveChanges();
                return RedirectToAction("Categories");
            }
            else
            {
                List<Category> AllCategories = dbContext.Categories
                .Include(c => c.Associations)
                .ThenInclude(p => p.AssociationProduct)
                .ToList();
                ViewBag.AllCategories = AllCategories;
                return View("Category");
            }
        }

        // Show page for single product
        [HttpGet("products/{id}")]
        public IActionResult ShowProduct(int id)
        {
            List<Category> AllCategories = dbContext.Categories
            .Include(c => c.Associations)
            .ThenInclude(p => p.AssociationProduct)
            .ToList();
            ViewBag.AllCategories = AllCategories;

            Product showProduct = dbContext.Products
            .Include(a => a.Associations)
            .ThenInclude(p => p.AssociationCategory)
            .FirstOrDefault(u => u.ProductId == id);
            return View("ShowProduct", showProduct);
        }

        // Show page for single category
        [HttpGet("categories/{id}")]
        public IActionResult ShowCategory(int id)
        {
            List<Product> AllProducts = dbContext.Products
            .Include(p => p.Associations)
            .ThenInclude(c => c.AssociationCategory)
            .ToList();
            ViewBag.AllProducts = AllProducts;

            List<Association> AllAssociations = dbContext.Associations
            .Include(c => c.AssociationCategory)
            .ThenInclude(p => p.CategoryId)
            .ToList();
            ViewBag.AllAssociations = AllAssociations;

            Category showCat = dbContext.Categories
            .Include(a => a.Associations)
            .ThenInclude(c => c.AssociationProduct)
            .FirstOrDefault(u => u.CategoryId == id);
            return View("ShowCategory", showCat);
        }

        // Post route for adding category to product
        [HttpPost("add/category")]
        public IActionResult AddCategorytoProduct(Association newAssociation)
        {
            if(ModelState.IsValid)
            {
                Product showProduct = dbContext.Products
                .FirstOrDefault(p => p.ProductId == newAssociation.ProductId);
                Category newCat = dbContext.Categories
                .FirstOrDefault(c => c.CategoryId == newAssociation.CategoryId);
                Association thisAssociation = new Association();
                thisAssociation.ProductId = newAssociation.ProductId;
                thisAssociation.CategoryId = newAssociation.CategoryId;
                thisAssociation.AssociationProduct = showProduct;
                thisAssociation.AssociationCategory = newCat;
                dbContext.Add(thisAssociation);
                dbContext.SaveChanges();
                return Redirect($"/products/{newAssociation.ProductId}");
            }
            else
            {
                Product showProduct = dbContext.Products
                .FirstOrDefault(p => p.ProductId == newAssociation.ProductId);
                ModelState.AddModelError("AssociationProduct", "Could not add category");
                return View("ShowProduct", showProduct);
            }
        }

        // Post route for adding product to category
        [HttpPost("add/product")]
        public IActionResult AddProducttoCategory(Association newAssociation)
        {
            if(ModelState.IsValid)
            {
                Category showCat = dbContext.Categories
                .FirstOrDefault(c => c.CategoryId == newAssociation.CategoryId);
                Product newProd = dbContext.Products
                .FirstOrDefault(p => p.ProductId == newAssociation.ProductId);
                Association thisAssociation = new Association();
                thisAssociation.ProductId = newAssociation.ProductId;
                thisAssociation.CategoryId = newAssociation.CategoryId;
                thisAssociation.AssociationProduct = newProd;
                thisAssociation.AssociationCategory = showCat;
                dbContext.Add(thisAssociation);
                dbContext.SaveChanges();
                return Redirect($"/categories/{newAssociation.CategoryId}");
            }
            else
            {
                Category showCat = dbContext.Categories
                .FirstOrDefault(c => c.CategoryId == newAssociation.CategoryId);
                ModelState.AddModelError("AssociationProduct", "Could not add category");
                return View("ShowCategory", showCat);
            }
        }
    }
}
