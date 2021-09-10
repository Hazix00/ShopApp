using ShopApp.Core.Models;
using ShopApp.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository context;
        public ProductManagerController()
        {
            context = new ProductRepository();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            var products = context.Collection().ToList();
            return View(products);
        }
        public ActionResult Create()
        {
            var product = new Product();
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            context.Insert(product);
            context.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string Id)
        {
            var product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            var productToUpdate = context.Find(Id);
            if (productToUpdate == null)
            {
                return HttpNotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            foreach (var prop in typeof(Product).GetProperties())
            {
                if (prop.Name == "Id") continue;
                prop.SetValue(productToUpdate, prop.GetValue(product));
            }

            context.Update(productToUpdate);
            context.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(string Id)
        {
            var product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ComfirmDelete(string Id)
        {
            var product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            context.Delete(product);
            context.Commit();
            return RedirectToAction("Index");
        }

    }
}