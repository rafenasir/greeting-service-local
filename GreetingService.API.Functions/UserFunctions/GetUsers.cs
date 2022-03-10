using System.IO;
using System.Net;
using System.Threading.Tasks;
using GreetingService.API.Functions.Authentication;
using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace GreetingService.API.Functions.UserFunctions
{
    public class GetUsers
    {
        private readonly ILogger<GetUsers> _logger;
        private readonly IAuthHandler _authHandler;
        private readonly IUserService _userService;


        public GetUsers(ILogger<GetUsers> log, IAuthHandler authHandler, IUserService userService)
        {
            _logger = log;
            _authHandler = authHandler;
            _userService = userService;
        }

        [FunctionName("GetUsers")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "User" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found")]

        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (!_authHandler.IsAuthorized(req))
            {
                return new UnauthorizedResult();
            }

            var users = await _userService.GetUsersAsync();

            return new OkObjectResult(users);

        }
    }
}

