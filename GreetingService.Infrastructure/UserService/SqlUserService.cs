using GreetingService.Core;
using GreetingService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.UserService
{
    public class SqlUserService : IUserService
    {
        private readonly GreetingDbContext _greetingDbContext;
        private readonly ILogger<SqlUserService> _logger;

        public SqlUserService(GreetingDbContext greetingDbContext, ILogger<SqlUserService> logger)
        {
            _greetingDbContext = greetingDbContext;
            _logger = logger;

        }


        public async Task CreateUser(User user)
        {
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            await _greetingDbContext.user.AddAsync(user);
            await _greetingDbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUser(string email)
        {
            var user = await _greetingDbContext.user.FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _greetingDbContext.user.ToListAsync();
        }

        public bool IsValidUser(string username, string password)
        {
            var user = _greetingDbContext.user.FirstOrDefault(x => x.Email.Equals(username));
            if (user != null && user.Password.Equals(password))
                return true;

            return false;
        }

        public async Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

      
    }
}
