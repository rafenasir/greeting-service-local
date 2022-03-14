using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using GreetingService.API.Functions.Authentication;
using GreetingService.Core;
using GreetingService.Core.Enums;
using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GreetingService.API.Functions.UserFunctions
{
    public class PostUser
    {
        private readonly ILogger<PostUser> _logger;
        private readonly IAuthHandler _authHandler;
        private readonly IMessagingService _messagingService;

        public PostUser(ILogger<PostUser> log, IAuthHandler authHandler, IMessagingService messagingService)
        {
            _logger = log;
            _authHandler = authHandler;
            _messagingService = messagingService;
        }

        [FunctionName("PostUser")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "User" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Description = "The OK response")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found")]

        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            if (!_authHandler.IsAuthorized(req))
            {
                return new UnauthorizedResult();
            }
            User user;

             try
            {
                user = JsonSerializer.Deserialize<User>(req.Body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            await _messagingService.SendAsync(user, MessagingServiceSubject.NewUser);

            return new AcceptedResult();
        }
    }
}

