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
    public class ApproveUserApi
    {
        private readonly ILogger<ApproveUserApi> _logger;
        private readonly IUserService _userService;


        public ApproveUserApi(ILogger<ApproveUserApi> log, IUserService userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("ApproveUserApi")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "User" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Accepted, Description = "Request accepted")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/approve/{code}")] HttpRequest req, string code)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                await _userService.ApproveUserAsync(code);
            }
            catch (UserNotFoundException e)
            {
                return new NotFoundObjectResult(e.Message);
            }

            return new AcceptedResult();


        }
    }
}

