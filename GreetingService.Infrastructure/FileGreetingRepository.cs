using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreetingService.Core;
using GreetingService.Core.Entities;
using System.Text.Json;

namespace GreetingService.Infrastructure
{
    public class FileGreetingRepository : IGreetingRepository
    {


        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true, };

        public FileGreetingRepository(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(path: _filePath))
                File.WriteAllText(_filePath, "[]");     //init file with empty json array
        }
       
        
        public async Task CreateAsync(Greeting greeting)
        {
            var fileData = File.ReadAllText(_filePath);
            var allGreetings = JsonSerializer.Deserialize<List<Greeting>>(fileData);
            if (allGreetings.Any(x => x.Id == greeting.Id))
                throw new Exception($"Greeting already exist again id {greeting.Id}");
            allGreetings.Add(greeting);
            var serializeGreetings = JsonSerializer.Serialize(allGreetings, _jsonSerializerOptions);
            File.WriteAllText(_filePath, serializeGreetings);
        }

        public async Task<Greeting> GetAsync(Guid id)
        {
            var content = File.ReadAllText(_filePath);
            var greetings = JsonSerializer.Deserialize<IList<Greeting>>(content);
            return greetings?.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Greeting>> GetAsync()
        {
            var content = File.ReadAllText(_filePath);
            var greetings = JsonSerializer.Deserialize<IList<Greeting>>(content);
            return greetings;
        }

        public async Task UpdateAsync(Greeting greeting)
        {
            var content = File.ReadAllText(_filePath);
            var greetings = JsonSerializer.Deserialize<IList<Greeting>>(content);
            var existingGreeting = greetings.FirstOrDefault(x => x.Id == greeting.Id);

            if (existingGreeting == null)
                throw new Exception($"Greeting with id: {greeting.Id} not found");

            existingGreeting.To = greeting.To;
            existingGreeting.From = greeting.From;
            existingGreeting.Message = greeting.Message;

            File.WriteAllText(_filePath, JsonSerializer.Serialize(greetings, _jsonSerializerOptions));
        }
        public async Task DeleteRecordAsync(Guid id)
        {

            var content = File.ReadAllText(_filePath);
            var greetings = JsonSerializer.Deserialize<IList<Greeting>>(content);
            var existingGreeting = greetings.FirstOrDefault(x => x.Id == id);

            if (existingGreeting == null)
                throw new Exception($"Greeting with id: {id} not found");

            else { greetings.Remove(existingGreeting); }
            File.WriteAllText(_filePath, JsonSerializer.Serialize(greetings, _jsonSerializerOptions));


        }

        public Task<IEnumerable<Greeting>> GetAsync(string from, string to)
        {
            throw new NotImplementedException();
        }
    }
}
