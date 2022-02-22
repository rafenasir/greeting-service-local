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
            var greeting = await _greetingDbContext.Greetings.FirstOrDefaultAsync(x => x.Id == id);             //Can use LINQ to query the db. EF Core will translate this to T-SQL before sending to the db
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
            var existingGreeting = await _greetingDbContext.Greetings.FirstOrDefaultAsync(x => x.Id == greeting.Id);            //get a handle on the greeting in the db
            if (existingGreeting == null)
                throw new Exception("Not found");

            existingGreeting.Message = greeting.Message;                                                                        //update the properties
            existingGreeting.To = greeting.To;
            existingGreeting.From = greeting.From;

            await _greetingDbContext.SaveChangesAsync();
        }
    }
}
