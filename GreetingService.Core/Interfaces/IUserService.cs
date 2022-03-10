using GreetingService.Core.Entities;
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
        public Task CreateUser(User user);
        public Task UpdateUser(User user);
        public Task DeleteUser(string email);
    }
}