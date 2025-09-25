using TestManager.Domain.DTO;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface ITransactionItemRepository : IGenericRepository<TransactionItem, int>
    {
        Task<AppointmentJoinProductsDTO> TransactionItemJoinProducts(AppointmentJoinProductsDTO appointmentJoinProductsDTO);

        Task<AppointmentAddProductsDTO> TransactionItemAddProducts(AppointmentAddProductsDTO appointmentAddProductsDTO);

        Task<AppointmentDeleteProductsDTO> AppointmentDeleteProducts(AppointmentDeleteProductsDTO appointmentDeleteProductsDTO);
    }
}
