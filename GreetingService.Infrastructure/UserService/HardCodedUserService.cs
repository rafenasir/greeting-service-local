using GreetingService.Core;
using GreetingService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure
{
    public class HardCodedUserService : IUserService
    {
        private static IDictionary<string, string> _users = new Dictionary<string, string>()
        {
            { "Rafe","summer2022" },
            { "Nasir","winter2022" },
        };

        public Task CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(string email)
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
            if (!_users.TryGetValue(username, out var storedPassword))              //user does not exist
                return false;

            if (!storedPassword.Equals(password))
                return false;

            return true;
        }

        public Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
