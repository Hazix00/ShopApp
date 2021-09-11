using ShopApp.Core.Models;
using ShopApp.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        InMemoryRepository<ProductCategory> context;
        public ProductCategoryManagerController()
        {
            context = new InMemoryRepository<ProductCategory>();
        }
        // GET: ProductCategoryManager
        public ActionResult Index()
        {
            var productCategories = context.Collection().ToList();
            return View(productCategories);
        }
        public ActionResult Create()
        {
            var productCategory = new ProductCategory();
            return View(productCategory);
        }
        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            context.Insert(productCategory);
            context.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string Id)
        {
            var productCategory = context.Find(Id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {
            var productCategoryToUpdate = context.Find(Id);
            if (productCategoryToUpdate == null)
            {
                return HttpNotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }

            foreach (var prop in typeof(ProductCategory).GetProperties())
            {
                if (prop.Name == "Id") continue;
                prop.SetValue(productCategoryToUpdate, prop.GetValue(productCategory));
            }

            context.Update(productCategoryToUpdate);
            context.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(string Id)
        {
            var productCategory = context.Find(Id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ComfirmDelete(string Id)
        {
            var productCategory = context.Find(Id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            context.Delete(productCategory);
            context.Commit();
            return RedirectToAction("Index");
        }
    }
}
