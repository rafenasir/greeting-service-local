using GreetingService.Core;
using GreetingService.Core.Entities;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure
{
    public class CosmosDbGreetingRepository : IGreetingRepository
    {
        private readonly CosmosClient _cosmosDbClient;

        public CosmosDbGreetingRepository(CosmosClient cosmosClient)
        {
            _cosmosDbClient = cosmosClient;
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
