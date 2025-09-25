using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.Uploader;

namespace TestManager.Service.Uploader
{
    public interface IAdviceService
    {
        Task<IEnumerable<AdviceDTO>> GetAdviceByPatientId(int patientId);

        Task<AdviceDTO> AddAdvice(AdviceDTO adviceDTO);
    }

    public class AdviceService(IAdviceRepository adviceRepository) : IAdviceService
    {
        public async Task<IEnumerable<AdviceDTO>> GetAdviceByPatientId(int patientId)
        {
            return await adviceRepository.GetAdviceByPatientId(patientId);
        }

        public async Task<AdviceDTO> AddAdvice(AdviceDTO adviceDTO)
        {
            return await adviceRepository.AddAdvice(adviceDTO);
        }
    }
}
