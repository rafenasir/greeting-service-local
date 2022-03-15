using System;
using System.Threading.Tasks;
using GreetingService.Core;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Functions.UserFunctions
{
    public class SbCreateUser
    {
        private readonly ILogger<SbCreateUser> _logger;
        private readonly IUserService _userService;


        public SbCreateUser(ILogger<SbCreateUser> log, IUserService userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("SbCreateUser")]
        public async Task Run([ServiceBusTrigger("main", "user_create", Connection = "ServiceBusConnectionString")] User user)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {user}");

            try
            {
                await _userService.CreateUserAsync(user);
            }
            catch(Exception ex) {
                _logger.LogError("failed to inser user in IUserService, e");
                    throw; }
        }
    }
}
