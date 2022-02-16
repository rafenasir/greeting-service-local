using Azure.Storage.Blobs;
using GreetingService.Core;
using GreetingService.Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure
{
    public class BlobGreetingRepository : IGreetingRepository
    {
        private const string _blobContainerName = "greetings";
        private readonly BlobContainerClient _blobContainerClient;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

        public BlobGreetingRepository(IConfiguration configuration)                 //ask for an IConfiguration here and dependency injection will provide it for us
        {
            var connectionString = configuration["LoggingStorageAccount"];          //get connection string from our app configuration
            _blobContainerClient = new BlobContainerClient(connectionString, _blobContainerName);
            _blobContainerClient.CreateIfNotExists();                               //create the container if it does not already exist
        }

        public async Task CreateAsync(Greeting greeting)
        {
            var blob = _blobContainerClient.GetBlobClient(greeting.Id.ToString());
            if (await blob.ExistsAsync())
            {
                throw new Exception($"Greeting with the ID {greeting.Id} already exists");
            }

            var greetingBinary = new BinaryData(greeting, jsonSerializerOptions);
            await blob.UploadAsync(greetingBinary);
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

        public Task UpdateAsync(Greeting greeting)
        {
            throw new NotImplementedException();
        }
    }
}
