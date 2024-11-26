﻿using SmartFarmManager.DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
       

        Task<int> CommitAsync();
        System.Threading.Tasks.Task BeginTransactionAsync();
        System.Threading.Tasks.Task RollbackAsync();
    }
}
