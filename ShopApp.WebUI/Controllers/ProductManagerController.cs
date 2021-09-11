using ShopApp.Core.Contracts;
using ShopApp.Core.Models;
using ShopApp.Core.ViewModels;
using ShopApp.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> productContext;
        IRepository<ProductCategory> productCategoryContext;
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            this.productContext = productContext;
            this.productCategoryContext = productCategoryContext;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            var products = productContext.Collection().ToList();
            return View(products);
        }
        public ActionResult Create()
        {
            var productManagerViewModel = new ProductManagerViewModel();
            productManagerViewModel.Product = new Product();
            productManagerViewModel.ProductCategories = productCategoryContext.Collection().ToList();
            return View(productManagerViewModel);
        }
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            if (file != null)
            {
                product.Image = $"{ product.Id }{ Path.GetExtension(file.FileName) }";
                file.SaveAs( $"{ Server.MapPath(@"/Content/ProductImages/") }{ product.Image }" );
            }
            productContext.Insert(product);
            productContext.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string Id)
        {
            var product = productContext.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var productManagerViewModel = new ProductManagerViewModel();
            productManagerViewModel.Product = product;
            productManagerViewModel.ProductCategories = productCategoryContext.Collection().ToList();
            return View(productManagerViewModel);
        }
        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
        {
            var productToUpdate = productContext.Find(Id);
            if (productToUpdate == null)
            {
                return HttpNotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (file != null)
            {
                string imagesPath = Server.MapPath(@"/Content/ProductImages/");
                string oldImage = $"{ imagesPath }{ productToUpdate.Image }";
                if (productToUpdate.Image != null && System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);
                }
                product.Image = $"{ product.Id }.{ Path.GetExtension(file.FileName) }";
                file.SaveAs($"{ imagesPath }{ product.Image }");
            }

            foreach (var prop in typeof(Product).GetProperties())
            {
                if (prop.Name == "Id") continue;
                prop.SetValue(productToUpdate, prop.GetValue(product));
            }

            productContext.Update(productToUpdate);
            productContext.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(string Id)
        {
            var product = productContext.Find(Id);
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
            var product = productContext.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            string imagesPath = Server.MapPath(@"/Content/ProductImages/");
            string oldImage = $"{ imagesPath }{ product.Image }";
            if (product.Image != null && System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }
            productContext.Delete(Id);
            productContext.Commit();
            return RedirectToAction("Index");
        }

    }
}