using ShopApp.Core.Models;
using ShopApp.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;
            if (items == null)
            {
                items = new List<T>();
            }
        }
        public void Commit()
        {
            cache[className] = items;
        }
        public void Insert(T t)
        {
            items.Add(t);
        }
        public void Update(T t)
        {
            T tToUpdate = items.FirstOrDefault(item => item.Id == t.Id);
            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception($"{className} not Found");
            }
        }
        public T Find(string Id)
        {
            T t = items.FirstOrDefault(item => item.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception($"{className} not Found");
            }
        }
        public void Delete(string Id)
        {
            T tToDelete = items.FirstOrDefault(item => item.Id == Id);
            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception($"{className} not Found");
            }
        }
        public IQueryable<T> Collection() => items.AsQueryable();
    }
}
