using GreetingService.Core;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure
{
    public class AppSettingsUserService : IUserService
    {
        private readonly IConfiguration _configuration;

        public AppSettingsUserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task ApproveUserAsync(string approvalCode)
        {
            throw new NotImplementedException();
        }

        public Task CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public bool IsValidUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsValidUserAsync(string username, string password)
        {
            var entries =  _configuration.AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
            if (entries.TryGetValue(username, out var storedPassword))
                return storedPassword == password;

            return false;
        }

        public Task RejectUserAsync(string approvalCode)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}