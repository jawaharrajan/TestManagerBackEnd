using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model.Uploader;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IAdviceRepository : IGenericRepository<Advice, int>
    {
        Task<IEnumerable<AdviceDTO>> GetAdviceByPatientId(int patientId);

        Task<AdviceDTO> AddAdvice(AdviceDTO adviceDTO);
    }
}
