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

namespace GreetingService.API.Functions
{
    public class PutGreeting
    {
        private readonly ILogger<PutGreeting> _logger;
        private readonly IGreetingRepository _greetingRepository;
        private readonly IAuthHandler _authhandler;

        public PutGreeting(ILogger<PutGreeting> logger, IGreetingRepository greetingRepository, IAuthHandler authhandler)
        {
            _greetingRepository = greetingRepository;
            _authhandler = authhandler;
            _logger = logger;
        }

        [FunctionName("PutGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Greeting" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Accepted, Description = "Accepted")]


        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "greeting/{id}")] HttpRequest req)
        {
            if (!_authhandler.IsAuthorized(req))
            
                return new UnauthorizedResult();

            

            _logger.LogInformation("C# HTTP trigger function processed a request.");


            var requestBody = await req.ReadAsStringAsync();
            //var greetingUpdate = JsonSerializer.Deserialize<Greeting>(requestBody);
            var greeting = JsonConvert.DeserializeObject<Greeting>(requestBody);

            try
            {
                _greetingRepository.Update(greeting);
            }
            catch
            {
                return new NotFoundResult();
            }

            return new AcceptedResult();
        }
    }
}
