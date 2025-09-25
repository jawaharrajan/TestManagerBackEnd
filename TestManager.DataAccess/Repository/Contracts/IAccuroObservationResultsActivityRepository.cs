using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model.Uploader;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IAccuroObservationResultsActivityRepository : IGenericRepository<AccuroLabObservationResultsActivity, int>
    {
        Task<int> AddAccuroLabObservationResult(AccuroLabObservationResultsActivityDTO accuroLabObsResultsActivityDTO);

        Task<IEnumerable<AccuroLabObservationResultsActivityDTO>> GetAccuroLabObsResultsActivityLogsByPatientId(int patientId);
    }
}
