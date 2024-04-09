using Microsoft.EntityFrameworkCore;
using RobotFx.Repositories.DBContext;
using RobotFx.Repositories.EntityFamework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
