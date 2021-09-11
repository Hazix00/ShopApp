using ShopApp.Core.Contracts;
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
        IRepository<ProductCategory> productCategoryContext;
        public ProductCategoryManagerController(IRepository<ProductCategory> productCategoryContext)
        {
            this.productCategoryContext = productCategoryContext;
        }
        // GET: ProductCategoryManager
        public ActionResult Index()
        {
            var productCategories = productCategoryContext.Collection().ToList();
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
            productCategoryContext.Insert(productCategory);
            productCategoryContext.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string Id)
        {
            var productCategory = productCategoryContext.Find(Id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {
            var productCategoryToUpdate = productCategoryContext.Find(Id);
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

            productCategoryContext.Update(productCategoryToUpdate);
            productCategoryContext.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(string Id)
        {
            var productCategory = productCategoryContext.Find(Id);
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
            var productCategory = productCategoryContext.Find(Id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            productCategoryContext.Delete(productCategory);
            productCategoryContext.Commit();
            return RedirectToAction("Index");
        }
    }
}
