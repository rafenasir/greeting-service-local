using Azure.Storage.Blobs;
using GreetingService.Core;
using GreetingService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure
{
    public class BlobUserService : IUserService
    {
        private const string _blobContainerName = "users";
        private const string _blobName = "users.json";

        private readonly BlobContainerClient _blobContainerClient;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

        public BlobUserService(IConfiguration configuration)
        {
            var connectionString = configuration["LoggingStorageAccount"];
            _blobContainerClient = new BlobContainerClient(connectionString, _blobContainerName);
            _blobContainerClient.CreateIfNotExists();

        }

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
            var blob = _blobContainerClient.GetBlobClient(_blobName);
            if (!blob.Exists())
            {
                return false;
            }
            var blobContent = blob.DownloadContent();
            var userDirectory = blobContent.Value.Content.ToObjectFromJson<IDictionary<string, string>>();
            if (userDirectory.TryGetValue(username, out var storedPassword))
            {
                if (storedPassword.Equals(password))
                {
                    return true;
                }
            }

            return false;

        }

        public Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
