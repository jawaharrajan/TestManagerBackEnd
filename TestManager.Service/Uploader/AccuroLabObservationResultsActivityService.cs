using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.Uploader;

namespace TestManager.Service.Uploader
{
    public interface IAccuroLabObservationResultsActivityService
    {
        Task<IEnumerable<AccuroLabObservationResultsActivityDTO>> GetAccuroLabObsResultsActivityLogsByPatientId(int patientId);
    }

    public class AccuroLabObservationResultsActivityService(IAccuroObservationResultsActivityRepository accuroObsResultsActivityRepository) : IAccuroLabObservationResultsActivityService
    {
        public async Task<IEnumerable<AccuroLabObservationResultsActivityDTO>> GetAccuroLabObsResultsActivityLogsByPatientId(int patientId)
        {
            return await accuroObsResultsActivityRepository.GetAccuroLabObsResultsActivityLogsByPatientId(patientId);
        }
    }
}
