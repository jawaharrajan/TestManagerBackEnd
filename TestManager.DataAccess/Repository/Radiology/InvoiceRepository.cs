using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Radiology
{
    public class InvoiceRepository(ApplicationDbContext _) : GenericRepository<Invoice, int>(_), IInvoiceRepository
    {
        public async Task<int> GetInvoiceIdAsync(int transactionID)
        {       
            var result = await (from i in _context.Invoice
                                where i.TransactionID == transactionID && i.IsOHIPInvoice == false
                                select (int?)i.InvoiceID)
                               .FirstOrDefaultAsync();

            return result ?? 5481;
            
        }
    }
}
