using TestManager.Domain.DTO;
using TestManager.Domain.DTO.Uploader;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IAccuroObservationRepository
    {
        Task<(List<AccuroLabPatientCollectionDTO> AccuroObservations, int TotalCount)> GetAccuroObservations(AccuroObservationFilterDto filter);
        Task<AccuroObservationsDropdownsDTO> GetDropdownsByReviewDate(DateTime reviewDate);
        Task<IEnumerable<AccuroLabObservationGroupDTO>> GetPatientOrdersResults(int patientId, IEnumerable<string> orderIds);
        Task UpdateObservationGroupsUploads(AccuroObservationGroupPatchDTO patchDTO);
        Task<IEnumerable<AccuroLogDTO>> GetPatientUploadedLogs(int patientId);
    }
}
