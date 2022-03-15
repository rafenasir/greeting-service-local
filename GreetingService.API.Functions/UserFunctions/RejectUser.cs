using System.IO;
using System.Net;
using System.Threading.Tasks;
using GreetingService.Core.Exceptions;
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
    public class RejectUser
    {
        private readonly ILogger<RejectUser> _logger;
        private readonly IUserService _userService;

        public RejectUser(ILogger<RejectUser> log, IUserService userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("RejectUser")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "User" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Accepted, Description = "Request accepted")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/reject/{code}")] HttpRequest req, string code)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                await _userService.RejectUserAsync(code);
            }
            catch (UserNotFoundException e)
            {
                return new NotFoundObjectResult(e.Message);
            }

            return new AcceptedResult();        //accepted status code means: We've received your request and it will be processed in due time, good response for asynchronous flows like this endpoints now has become when using IMessagingService.SendAsync()
        }
    }
}

