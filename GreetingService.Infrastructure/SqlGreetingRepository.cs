using GreetingService.Core;
using GreetingService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure
{
    public class SqlGreetingRepository : IGreetingRepository
    {
        private readonly GreetingDbContext _greetingDbContext;
        public SqlGreetingRepository(GreetingDbContext greeting)
        {
            _greetingDbContext = greeting;
        }
        
        public Task CreateAsync(Greeting greeting)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRecordAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Greeting> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Greeting>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Greeting>> GetAsync(string from, string to)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Greeting greeting)
        {
            throw new NotImplementedException();
        }
    }
}
