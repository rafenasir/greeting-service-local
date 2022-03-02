using GreetingService.Core.Entities;
using GreetingService.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure
{
    public class SqlInvoiceService : IInvoiceService
    {
        private readonly GreetingDbContext _greetingDbContext;
        public SqlInvoiceService(GreetingDbContext greetingDbContext)
        {
            _greetingDbContext = greetingDbContext;
        }

        public async Task CreateOrUpdateInvoiceAsync(Invoice invoice)
        {
            var existingInvoice = await _greetingDbContext.Invoices.FirstOrDefaultAsync(x => x.Year == invoice.Year && x.Month == invoice.Month && x.User.Email.Equals(invoice.User.Email));
            if (existingInvoice == null)
            {
                await _greetingDbContext.AddAsync(invoice);
                await _greetingDbContext.SaveChangesAsync();
            }
            else
            {
                existingInvoice.Greetings = invoice.Greetings;
                existingInvoice.TotalCost = invoice.TotalCost;
                await _greetingDbContext.SaveChangesAsync();
            }
        }

        public async Task<Invoice> GetInvoiceAsync(int year, int month, string email)
        {
            var invoice = await _greetingDbContext.Invoices
                .Include(x => x.Greetings)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Year == year && x.Month == month && x.User.Email.Equals(email));
            return invoice;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync(int year, int month)
        {
            var invoices = await _greetingDbContext.Invoices
                            .Include(x => x.Greetings)
                            .Include(x => x.User)
                            .Where(x => x.Year == year && x.Month == month)
                            .ToListAsync();

            return invoices;
        }
    }
}
