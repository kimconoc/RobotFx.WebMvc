using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotFx.Repositories.EntityFamework.Interface
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> AsQueryable();
        IEnumerable<T> AsEnumerable();
        T GetById(int id);
        bool Insert(T entity);
        bool Update(T entity);
        bool SoftDelete(T entity);
        bool Delete(T entity);
    }
}
