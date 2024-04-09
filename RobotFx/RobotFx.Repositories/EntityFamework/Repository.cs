using Microsoft.EntityFrameworkCore;
using RobotFx.Repositories.DBContext;
using RobotFx.Repositories.EntityFamework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RobotFx.Repositories.EntityFamework
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CommonDBContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(CommonDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> AsQueryable()
        {
            return _context.Set<T>().AsQueryable();
        }

        public IEnumerable<T> AsEnumerable()
        {
            return _dbSet;
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public bool Insert(T entity)
        {
            bool isInsert = false;
            _dbSet.Add(entity);
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Added)
                isInsert = true;

            return isInsert;
        }

        public bool Update(T entity)
        {
            bool isUpdate = false;
            _context.Entry(entity).State = EntityState.Modified;
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Modified)
                isUpdate = true;

            return isUpdate;
        }

        public bool Delete(T entity)
        {
            bool isDelete = false;
            _dbSet.Remove(entity);
            _context.Entry(entity).State = EntityState.Modified;
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Deleted)
                isDelete = true;

            return isDelete;
        }
    }
}
