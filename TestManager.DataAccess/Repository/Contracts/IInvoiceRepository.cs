using TestManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IInvoiceRepository : IGenericRepository<Invoice, int>
    {
        Task<int> GetInvoiceIdAsync(int TransactionID);
    }
}
