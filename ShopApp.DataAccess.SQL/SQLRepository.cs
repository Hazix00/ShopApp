using ShopApp.Core.Contracts;
using ShopApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext dataContext;
        internal DbSet<T> dbSet;
        public SQLRepository(DataContext context)
        {
            dataContext = context;
            dbSet = dataContext.Set<T>();
        }

        public IQueryable<T> Collection() => dbSet;
        
        public void Commit()
        {
            dataContext.SaveChanges();
        }

        public void Delete(string Id)
        {
            T tToDelete = Find(Id);
            if (dataContext.Entry(tToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(tToDelete);
            }
            dbSet.Remove(tToDelete);
        }

        public T Find(string Id)
        {
           return dbSet.FirstOrDefault(item => item.Id == Id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            dbSet.Attach(t);
            dataContext.Entry(t).State = EntityState.Modified;
        }
    }
}
