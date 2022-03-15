using System;
using System.Threading.Tasks;
using GreetingService.Core;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Functions.UserFunctions
{
    public class SbUpdateUser
    {
        private readonly ILogger<SbUpdateUser> _logger;
        private readonly IUserService _userService;


        public SbUpdateUser(ILogger<SbUpdateUser> log, IUserService userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("SbUpdateUser")]
        public async Task Run([ServiceBusTrigger("main", "user_put", Connection = "ServiceBusConnectionString")] User user)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {user}");

            try
            {
                await _userService.UpdateUserAsync(user);

            }
            catch (Exception e)
            {
                _logger.LogError("Failed to update User in IUserService", e);

            }
        }
    }
}
