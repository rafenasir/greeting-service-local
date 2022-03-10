using System;
using System.Linq;
using System.Threading.Tasks;
using GreetingService.Core;
using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Functions.InvoiceFunctions
{
    public class ComputeInvoices
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IGreetingRepository _greetingRepository;
        private readonly IUserService _userService;

        public ComputeInvoices(IInvoiceService invoiceService, IGreetingRepository greetingRepository, IUserService userService)
        {
            _invoiceService = invoiceService;
            _greetingRepository = greetingRepository;
            _userService = userService;
        }


        [FunctionName("ComputeInvoices")]
        public async Task Run([TimerTrigger("*/5 * * * * ")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var greetings = await _greetingRepository.GetAsync();

            var greetingsGrpuedByInvoice = greetings.GroupBy(x => new { x.From, x.Timestamp.Year, x.Timestamp.Month });

            foreach (var group in greetingsGrpuedByInvoice)
            {
                var user = await _userService.GetUserAsync(group.Key.From);
                var invoice = new Invoice
                {
                    Greetings = group,
                    Month = group.Key.Month,
                    Year = group.Key.Year,
                    User = user,
                };

                await _invoiceService.CreateOrUpdateInvoiceAsync(invoice);
            }
        }
    }
}
