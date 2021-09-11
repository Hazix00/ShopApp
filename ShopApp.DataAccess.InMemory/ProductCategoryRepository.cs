using ShopApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }
        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }
        public void Insert(ProductCategory productCategory)
        {
            productCategories.Add(productCategory);
        }
        public void Update(ProductCategory productCategory)
        {
            var productCategoryToUpdate = productCategories.FirstOrDefault(p => p.Id == productCategory.Id);
            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = productCategory;
            }
            else
            {
                throw new Exception("Product Category not found!");
            }
        }
        public void Delete(ProductCategory productCategory)
        {
            var productCategoryToDelete = productCategories.FirstOrDefault(p => p.Id == productCategory.Id);
            if (productCategoryToDelete != null)
            {
                productCategories.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception("ProductCategory not found!");
            }
        }
        public ProductCategory Find(string Id)
        {
            var productCategory = productCategories.FirstOrDefault(p => p.Id == Id);
            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception("Product Category not found!");
            }
        }
        public IQueryable<ProductCategory> Collection() => productCategories.AsQueryable();
    }
}
