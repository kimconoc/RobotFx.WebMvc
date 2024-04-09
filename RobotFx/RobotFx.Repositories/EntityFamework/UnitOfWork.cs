using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using RobotFx.Repositories.EntityFamework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotFx.Repositories.DBContext;

namespace RobotFx.Repositories.EntityFamework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CommonDBContext _context;
        private IDbContextTransaction _currentTransaction;

        public UnitOfWork(CommonDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("Transaction already exists.");
            }

            _currentTransaction = _context.Database.BeginTransaction();
        }

        public int Commit(bool isTrackChanged = true)
        {
            try
            {
                if (isTrackChanged)
                {
                    _context.ChangeTracker.DetectChanges();
                }

                int affectedRows = _context.SaveChanges();
                _currentTransaction?.Commit();
                return affectedRows;
            }
            catch (Exception)
            {
                RollBack();
                throw;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public async Task<int> CommitAsync(bool isTrackChanged = true)
        {
            try
            {
                if (isTrackChanged)
                {
                    _context.ChangeTracker.DetectChanges();
                }

                int affectedRows = await _context.SaveChangesAsync();
                _currentTransaction?.Commit();
                return affectedRows;
            }
            catch (Exception)
            {
                RollBack();
                throw;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public void RollBack()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
