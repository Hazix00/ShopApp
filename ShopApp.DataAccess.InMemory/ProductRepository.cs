using ShopApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if (products == null)
            {
                products = new List<Product>();
            }
        }
        public void Commit()
        {
            cache["products"] = products;
        }
        public void Insert(Product product)
        {
            products.Add(product);
        }
        public void Update(Product product)
        {
            var productToUpdate = products.FirstOrDefault(p => p.Id == product.Id);
            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }
        public void Delete(Product product)
        {
            var productToDelete = products.FirstOrDefault(p => p.Id == product.Id);
            if (productToDelete != null)
            {
                productToDelete = product;
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }
        public Product Find(string Id)
        {
            var product = products.FirstOrDefault(p => p.Id == Id);
            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }
        public IQueryable<Product> Collection() => products.AsQueryable();
    }
}
