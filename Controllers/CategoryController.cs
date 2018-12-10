using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ProductsAndCategories.Controllers
{
    public class CategoryController : Controller
    {
        private ProjectContext DbContext;
        public CategoryController(ProjectContext context)
        {
            DbContext = context;
        }

        [HttpGet]
        [Route("categories")]
        public IActionResult CategoriesList()
        {
            ViewBag.allcategories = DbContext.Categories.ToList();
            return View();
        }

        [HttpPost]
        [Route("categories/new")]
        public IActionResult AddCategory(Category newcategory)
        {
            if(ModelState.IsValid)
            {
                DbContext.Categories.Add(newcategory);
                DbContext.SaveChanges();
                return RedirectToAction("CategoriesList");
            }
            ViewBag.allcategories = DbContext.Products.ToList();
            return View("CategoriesList");
        }
        
        [HttpGet]
        [Route("categories/delete/{target}")]
        public IActionResult deleteCategories(int target)
        {
            Category deleteitem = DbContext.Categories.SingleOrDefault(d => d.CategoryId == target);
            DbContext.Categories.Remove(deleteitem);
            DbContext.SaveChanges();
            return RedirectToAction("CategoriesList");
        }

        [HttpGet]
        [Route("categories/{target}")]
        public IActionResult CategoryDetail(int target)
        {
            var CategorieswithProducts = DbContext.Categories
                .Where(c => c.CategoryId == target)
                .Include(c => c.ProductsInCategory)
                .ThenInclude(p => p.Product).SingleOrDefault();
            List<Association> selectedAssociations = CategorieswithProducts.ProductsInCategory;

            List<Product> products = DbContext.Products
                .Include(a => a.ProductCategories)
                .Where(a => a.ProductCategories.All(c => c.CategoryId != target))
                .ToList();
            ViewBag.products = products;
            return View(CategorieswithProducts);
        }

        [HttpPost]
        [Route("categories/associations/add/{target}")]
        public IActionResult AddCategoryAssociation(int target, int product)
        {
            Association newdata = new Association();
            newdata.CategoryId = target;
            newdata.ProductId = product;
            DbContext.Associations.Add(newdata);
            DbContext.SaveChanges();
            return RedirectToAction("CategoryDetail");
        }
        [HttpGet]
        [Route("categories/associations/delete/{aId}")]
        public IActionResult DeleteCategoryAssociation(int aId)
        {
            Association deleteitem = DbContext.Associations
                .SingleOrDefault(a => a.AssociationId == aId);
            int CId = deleteitem.CategoryId;
            DbContext.Associations.Remove(deleteitem);
            DbContext.SaveChanges();
            return RedirectToAction("CategoryDetail", new {target = CId});
        }

    }
}