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

namespace GreetingService.API.Functions
{
    public class PutGreeting
    {
        private readonly ILogger<PutGreeting> _logger;
        private readonly IGreetingRepository _greetingRepository;

        public PutGreeting(ILogger<PutGreeting> logger, IGreetingRepository greetingRepository)
        {
            _greetingRepository = greetingRepository;
            _logger = logger;
        }

        [FunctionName("PutGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Greeting" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Accepted, Description = "Accepted")]


        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "greeting/{id}")] HttpRequest req)
        {
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
