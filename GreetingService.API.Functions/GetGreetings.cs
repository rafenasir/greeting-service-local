using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using GreetingService.API.Functions.Authentication;
using GreetingService.Core;
using GreetingService.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace GreetingService.API.Functions
{
    public class GetGreetings
    {
        private readonly ILogger<GetGreetings> _logger;
        private readonly IGreetingRepository _greetingRepository;
        private readonly IAuthHandler _iauthHandler;

        public GetGreetings(ILogger<GetGreetings> log, IGreetingRepository greetingRepository, IAuthHandler iauthHandler)
        {
            _logger = log;
            _greetingRepository = greetingRepository;
            _iauthHandler = iauthHandler;
        }

        [FunctionName("GetGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Greeting" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Greeting>), Description = "The OK response")]


        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "greeting")] HttpRequest req)
        {
            if (!_iauthHandler.IsAuthorized(req))
            {
                return new UnauthorizedResult();
            }
            _logger.LogInformation("C# HTTP trigger function processed a request.");



            var from = req.Query["from"];
            var to = req.Query["to"];

            var greetings = await _greetingRepository.GetAsync(from, to);

            return new OkObjectResult(greetings);
        }
    }
}

