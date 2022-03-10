﻿using GreetingService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Core.Interfaces
{
    public interface IUserService
    {
        public bool IsValidUser(string username, string password);
        public Task<User> GetUserAsync(string email);
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task CreateUserAsync(User user);
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(string email);
        Task<bool> IsValidUserAsync(string v1, string v2);
    }
}