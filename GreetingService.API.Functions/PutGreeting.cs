using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GreetingService.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Net;
using GreetingService.Core.Entities;
using GreetingService.API.Functions.Authentication;
using GreetingService.Core.Interfaces;
using GreetingService.Core.Enums;

namespace GreetingService.API.Functions
{
    public class PutGreeting
    {
        private readonly ILogger<PutGreeting> _logger;
        private readonly IMessagingService _messagingService;
        private readonly IAuthHandler _authhandler;

        public PutGreeting(ILogger<PutGreeting> logger, IMessagingService messagingService, IAuthHandler authhandler)
        {
            _messagingService = messagingService;
            _authhandler = authhandler;
            _logger = logger;
        }

        [FunctionName("PutGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Greeting" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Accepted, Description = "Accepted")]


        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "greeting")] HttpRequest req)
        {
            if (!await _authhandler.IsAuthorizedAsync(req))
            
                return new UnauthorizedResult();

            

            _logger.LogInformation("C# HTTP trigger function processed a request.");


            var requestBody = await req.ReadAsStringAsync();
            var greeting = JsonConvert.DeserializeObject<Greeting>(requestBody);

            try
            {
               await _messagingService.SendAsync(greeting, MessagingServiceSubject.UpdateGreeting);
            }
            catch
            {
                return new NotFoundResult();
            }

            return new AcceptedResult();
        }
    }
}
