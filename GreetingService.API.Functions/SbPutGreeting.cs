using System;
using System.Threading.Tasks;
using GreetingService.Core;
using GreetingService.Core.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Functions
{
    public class SbPutGreeting
    {
        private readonly ILogger<SbPutGreeting> _logger;
        private readonly IGreetingRepository _greetingRepository;

        public SbPutGreeting(ILogger<SbPutGreeting> log, IGreetingRepository greetingRepository)
        {
            _logger = log;
            _greetingRepository = greetingRepository;
        }

        [FunctionName("SbPutGreeting")]
        public async Task Run([ServiceBusTrigger("main", "greeting_put", Connection = "ServiceBusConnectionString")] Greeting greeting)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {greeting}");
            try
            {
                await _greetingRepository.UpdateAsync(greeting);

            }
            catch (Exception e)
            {
                _logger.LogError("Failed to update the Greeting", e);
                throw;
            }

        }
    }
}
