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

        public void CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string email)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string email)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public bool IsValidUser(string username, string password)
        {
            var entries = _configuration.AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
            if (entries.TryGetValue(username, out var storedPassword))
                return storedPassword == password;

            return false;
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}