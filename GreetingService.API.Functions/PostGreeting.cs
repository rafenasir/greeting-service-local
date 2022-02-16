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
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace GreetingService.API.Functions
{
    public class PostGreeting
    {
        private readonly ILogger<PostGreeting> _logger;
        private readonly IGreetingRepository _greetingRepository;
        private readonly IAuthHandler _authhandler;


        public PostGreeting(ILogger<PostGreeting> log, IGreetingRepository greetingRepository, IAuthHandler iAuthHandler)
        {
            _logger = log;
            _greetingRepository = greetingRepository;
            _authhandler = iAuthHandler;
        }

        [FunctionName("PostGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Greetings" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Accepted, Description = "Accepted")]

        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "greeting")] HttpRequest req)
        {
            if (!_authhandler.IsAuthorized(req))
            {
                return new UnauthorizedResult();   
            }
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //var greeting = JsonSerializer.Deserialize<Greeting>(greeting);
            var greeting = JsonConvert.DeserializeObject<Greeting>(requestBody);

            try
            {
                await _greetingRepository.CreateAsync(greeting);
            }
            catch
            {
                return new ConflictResult();
            }

            return new AcceptedResult();
        }
    }
}

