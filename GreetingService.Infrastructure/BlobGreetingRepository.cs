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

        public Task<Greeting> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Greeting>> GetAsync()
        {
            var greetings = new List<Greeting>();
            var blobs = _blobContainerClient.GetBlobsAsync();
            await foreach (var blob in blobs)
            {
                var blobClient = _blobContainerClient.GetBlobClient(blob.Name);
                var blobContent = await blobClient.DownloadContentAsync();
                var greeting = blobContent.Value.Content.ToObjectFromJson<Greeting>();
                greetings.Add(greeting);
            }
            return greetings;
        }

        public async Task UpdateAsync(Greeting greeting)
        {
            var blobClient = _blobContainerClient.GetBlobClient(greeting.Id.ToString());
            await blobClient.DeleteIfExistsAsync();
            var greetingBinary = new BinaryData(greeting, jsonSerializerOptions);
            await blobClient.UploadAsync(greetingBinary);
        }

        public Task DeleteRecordAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
