using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ProductsAndCategories.Controllers
{
    public class ProductController : Controller
    {
        private ProjectContext DbContext;
        public ProductController(ProjectContext context)
        {
            DbContext = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult ProductsList()
        {
            ViewBag.allproducts = DbContext.Products.ToList();
            return View();
        }

        [HttpPost]
        [Route("products/new")]
        public IActionResult AddProduct(Product newproduct)
        {
            if(ModelState.IsValid)
            {
                DbContext.Products.Add(newproduct);
                DbContext.SaveChanges();
                return RedirectToAction("ProductsList");
            }
            ViewBag.allproducts = DbContext.Products.ToList();
            return View("ProductsList");
        }
        
        [HttpGet]
        [Route("products/delete/{target}")]
        public IActionResult deleteProduct(int target)
        {
            Product deleteitem = DbContext.Products.SingleOrDefault(d => d.ProductId == target);
            DbContext.Products.Remove(deleteitem);
            DbContext.SaveChanges();
            return RedirectToAction("ProductsList");
        }

        [HttpGet]
        [Route("products/{target}")]
        public IActionResult ProductDetail(int target)
        {
            var productwithCategories = DbContext.Products
                .Where(p => p.ProductId == target)
                .Include(p => p.ProductCategories)
                .ThenInclude(p => p.Category).SingleOrDefault();
            List<Association> selectedAssociations = productwithCategories.ProductCategories;

            IEnumerable<Category> categories = DbContext.Categories
            //IEnumerable returns data in JSON format
                .Include(c => c.ProductsInCategory)
                .Where(c => c.ProductsInCategory.All(p => p.ProductId != target));
                //.Where(c => !c.ProductsInCategory.Any(p => p.ProductId == target));
            ViewBag.catergories = categories;
            return View(productwithCategories);
        }

        [HttpPost]
        [Route("products/associations/add/{target}")]
        public IActionResult AddProductAssociation(int target, int category)
        {
            Association newdata = new Association();
            newdata.ProductId = target;
            newdata.CategoryId = category;
            DbContext.Associations.Add(newdata);
            DbContext.SaveChanges();
            return RedirectToAction("ProductDetail");
        }

        [HttpGet]
        [Route("products/associations/delete/{aId}")]
        public IActionResult DeleteProductAssociation(int aId)
        {
            Association deleteitem = DbContext.Associations
                .SingleOrDefault(a => a.AssociationId == aId);
            int PId = deleteitem.ProductId;
            DbContext.Associations.Remove(deleteitem);
            DbContext.SaveChanges();
            return RedirectToAction("ProductDetail", new {target = PId});
        }
    }
}
