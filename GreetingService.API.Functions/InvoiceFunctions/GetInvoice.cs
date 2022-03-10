using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Net;
using GreetingService.API.Functions.Authentication;
using GreetingService.Core.Interfaces;
using GreetingService.Core.HelperFunctions;

namespace GreetingService.API.Functions.InvoiceFunctions
{
    public class GetInvoice
    {

        private readonly ILogger<GetInvoice> _logger;
        private readonly IAuthHandler _authHandler;
        private readonly IInvoiceService _invoiceService;
        public GetInvoice(ILogger<GetInvoice> log, IAuthHandler authHandler, IInvoiceService invoiceService)
        {
            _logger = log;
            _authHandler = authHandler;
            _invoiceService = invoiceService;
        }


        [FunctionName("GetInvoice")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Invoice" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Accepted, Description = "Accepted")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "invoice/{year}/{month}/{email}")] HttpRequest req, int year, int month, string email
             )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (!await _authHandler.IsAuthorizedAsync(req))
            {
                return new UnauthorizedResult();
            }

            if (!EmailValidator.IsValidEmail(email))
            {
                return new BadRequestObjectResult($"{email} in not a valid email address with correct format.");
            }

            var invoice = await _invoiceService.GetInvoiceAsync(year, month, email);
            return new OkObjectResult(invoice);
        }
    }
}
