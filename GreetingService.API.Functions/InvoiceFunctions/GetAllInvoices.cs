using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GreetingService.API.Functions.Authentication;
using GreetingService.Core.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Net;

namespace GreetingService.API.Functions.InvoiceFunctions
{
    public class GetAllInvoices
    {
        private readonly ILogger<GetAllInvoices> _logger;
        private readonly IAuthHandler _authHandler;
        private readonly IInvoiceService _invoiceService;

        public GetAllInvoices(ILogger<GetAllInvoices> log, IAuthHandler authHandler, IInvoiceService invoiceService)
        {
            _logger = log;
            _authHandler = authHandler;
            _invoiceService = invoiceService;
        }

        [FunctionName("GetAllInvoices")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Invoice" })]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Accepted, Description = "Accepted")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "invoice/{year}/{month}")] HttpRequest req, int year, int month, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!_authHandler.IsAuthorized(req))
            {
                return new UnauthorizedResult();
            }

            var invoices = await _invoiceService.GetInvoicesAsync(year, month);
            return new OkObjectResult(invoices);
        }
    }
}
