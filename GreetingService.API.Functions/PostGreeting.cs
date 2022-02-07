using System.IO;
using System.Net;
using System.Threading.Tasks;
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

        public PostGreeting(ILogger<PostGreeting> log, IGreetingRepository greetingRepository)
        {
            _logger = log;
            _greetingRepository = greetingRepository;
        }

        [FunctionName("PostGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Greetings" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Accepted, Description = "Accepted")]

        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "greeting")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //var greeting = JsonSerializer.Deserialize<Greeting>(greeting);
            var greeting = JsonConvert.DeserializeObject<Greeting>(requestBody);

            try
            {
                _greetingRepository.Create(greeting);
            }
            catch
            {
                return new ConflictResult();
            }

            return new AcceptedResult();
        }
    }
}

