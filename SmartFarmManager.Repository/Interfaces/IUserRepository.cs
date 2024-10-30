﻿using SmartFarmManager.DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Repository.Interfaces
{
    public interface IUserRepository : IRepositoryBaseAsync<User>
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserByIdAsync(int id);
    }
}
