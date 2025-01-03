﻿using Microsoft.EntityFrameworkCore.Storage;
using SmartFarmManager.DataAccessObject.Models;
using SmartFarmManager.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartFarmContext _context;
        private IDbContextTransaction _currentTransaction;

        public IUserRepository Users { get;}
        public ITaskRepository Tasks { get;}
        public ITaskTypeRepository TaskTypes { get;}
        public IStatusRepository Statuses { get; }
        public IStatusLogRepository StatusLogs { get; }
        public ICageRepository Cages { get; }

        public UnitOfWork(SmartFarmContext context, IUserRepository users, ITaskTypeRepository taskTypes,ITaskRepository tasks, IStatusRepository statuses, IStatusLogRepository statusLogs, ICageRepository cages)
        {
            _context = context;
            Users = users;
            TaskTypes = taskTypes;
            Tasks = tasks;
            Statuses = statuses;
            StatusLogs = statusLogs;
            Cages = cages;
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _context.Dispose();
        }

        public async System.Threading.Tasks.Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();

                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }

                return result;
            }
            catch
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync();
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }

                throw;
            }
        }

        public async System.Threading.Tasks.Task RollbackAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public SmartFarmContext GetDbContext()
        {
            return _context;
        }
    }
}
