using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogApp.Repositories
{
    public class Repository<T> : IDisposable where T : class
    {
        EF.DataContext db;
        DbSet<T> Table;

        public Repository()
        {
            db = new EF.DataContext();
            Table = db.Set<T>();
        }

        ~Repository()
        {
            Dispose();
        }

        #region Query

        public List<T> All(Expression<Func<T, object>> include = null)
        {
            if (include != null)
                return Table.Include(include).ToList();
            return Table.ToList();
        }
        public List<T> Paging(Func<T, object> orderby, uint PageNumber, int Count, Expression<Func<T, object>> include = null)
        {
            if (include != null)
                return Table.Include(include).OrderByDescending(orderby).Skip(Convert.ToInt32((PageNumber - 1) * Count)).Take(Count).ToList();
            return Table.OrderByDescending(orderby).Skip(Convert.ToInt32((PageNumber - 1) * Count)).Take(Count).ToList();
        }

        public List<T> Where(Func<T, bool> where, Expression<Func<T, object>> include = null)
        {
            if (include != null)
                return Table.Include(include).Where(where).ToList();
            return Table.Where(where).ToList();
        }

        public T First(Expression<Func<T, object>> include = null)
        {
            if (include != null)
                return Table.Include(include).FirstOrDefault();
            return Table.FirstOrDefault();
        }

        public T First(Func<T, bool> first, Expression<Func<T, object>> include = null)
        {
            if (include != null)
                return Table.Include(include).FirstOrDefault(first);
            return Table.FirstOrDefault(first);
        }

        public T Single(Func<T, bool> single, Expression<Func<T, object>> include = null)
        {
            if (include != null)
                return Table.Include(include).SingleOrDefault(single);
            return Table.SingleOrDefault(single);
        }

        public int Count()
        {
            return Table.Count();
        }
        
        #endregion

        #region Proc

        public bool Add(T entity)
        {
            Table.Add(entity);
            return Save();
        }

        public bool Update(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Modified;
            return Save();
        }

        public bool Delete(T entity)
        {
            Table.Remove(entity);
            return Save();
        }

        #endregion

        public DbSet<T> GetTable()
        {
            return Table;
        }

        private bool Save()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(Table);
            db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
