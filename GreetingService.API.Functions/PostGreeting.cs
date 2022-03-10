using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using GreetingService.API.Functions.Authentication;
using GreetingService.Core;
using GreetingService.Core.Entities;
using GreetingService.Core.Enums;
using GreetingService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GreetingService.API.Functions
{
    public class PostGreeting
    {
        private readonly ILogger<PostGreeting> _logger;
        private readonly IMessagingService _messagingService;
        private readonly IAuthHandler _authhandler;


        public PostGreeting(ILogger<PostGreeting> log, IMessagingService messagingService, IAuthHandler iAuthHandler)
        {
            _logger = log;
            _messagingService = messagingService;
            _authhandler = iAuthHandler;
        }

        [FunctionName("PostGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Greetings" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Accepted, Description = "Accepted")]

        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "greeting")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (!await _authhandler.IsAuthorizedAsync(req))
            {
                return new UnauthorizedResult();   
            }
            Greeting greeting;

            try
            {
                var body = await req.ReadAsStringAsync();
                greeting = JsonSerializer.Deserialize<Greeting>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            //var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //var greeting = JsonConvert.DeserializeObject<Greeting>(requestBody);

            try
            {
                await _messagingService.SendAsync(greeting, MessagingServiceSubject.NewGreeting);
            }
            catch
            {
                return new ConflictResult();
            }

            return new AcceptedResult();
        }
    }
}

