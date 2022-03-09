using GreetingService.Core;
using GreetingService.Core.Entities;
using Microsoft.EntityFrameworkCore;
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
        
        public async Task CreateAsync(Greeting greeting)
        {
            await _greetingDbContext.Greetings.AddAsync(greeting);
            await _greetingDbContext.SaveChangesAsync();
        }

        public Task DeleteRecordAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Greeting> GetAsync(Guid id)
        {
            var greeting = await _greetingDbContext.Greetings.FirstOrDefaultAsync(x => x.Id == id);             
            if (greeting == null)
                throw new Exception("Not found");

            return greeting;
        }

        public async Task<IEnumerable<Greeting>> GetAsync()
        {
            return await _greetingDbContext.Greetings.ToListAsync();
        }

        public Task<IEnumerable<Greeting>> GetAsync(string from, string to)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Greeting greeting)
        {
            var existingGreeting = await _greetingDbContext.Greetings.FirstOrDefaultAsync(x => x.Id == greeting.Id);            
            if (existingGreeting == null)
                throw new Exception("Not found");

            existingGreeting.Message = greeting.Message;                                                                   
            existingGreeting.To = greeting.To;
            existingGreeting.From = greeting.From;
            existingGreeting.Timestamp = greeting.Timestamp;

            await _greetingDbContext.SaveChangesAsync();
        }
    }
}
