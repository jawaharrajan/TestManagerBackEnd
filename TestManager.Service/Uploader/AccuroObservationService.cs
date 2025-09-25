using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using TestManager.Interfaces;

namespace TestManager.Service.Uploader
{
    public interface IAccuroObservationService
    {
        Task<AccuroObservationsDropdownsDTO> GetDropdownsByReviewDate(DateTime reviewDate);
        Task<(List<AccuroLabPatientCollectionDTO> AccuroObservations, int TotalCount)> GetAccuroObservations(AccuroObservationFilterDto filter);
        Task<IEnumerable<AccuroLabObservationGroupDTO>> GetPatientOrdersResults(int patientId, IEnumerable<string> orderIds);
        Task UpdateObservationGroupsUploads(AccuroObservationGroupPatchDTO patchDTO);
        Task<IEnumerable<AccuroLogDTO>> GetPatientUploadedLogs(int patientId);
    }

    public class AccuroObservationService(IAccuroObservationRepository accuroObservationRepository,
        IUserContextService userContextService,
        IAccuroObservationResultsActivityRepository accuroObsResultsActivityRepository) : IAccuroObservationService
    {
        public async Task<(List<AccuroLabPatientCollectionDTO> AccuroObservations, int TotalCount)> GetAccuroObservations(AccuroObservationFilterDto filter)
        {
            return await accuroObservationRepository.GetAccuroObservations(filter);
        }

        public async Task<AccuroObservationsDropdownsDTO> GetDropdownsByReviewDate(DateTime reviewDate)
        {
            return await accuroObservationRepository.GetDropdownsByReviewDate(reviewDate);
        }

        public async Task<IEnumerable<AccuroLabObservationGroupDTO>> GetPatientOrdersResults(int patientId, IEnumerable<string> orderIds) 
        {
            return await accuroObservationRepository.GetPatientOrdersResults(patientId, orderIds);
        }

        public async Task UpdateObservationGroupsUploads(AccuroObservationGroupPatchDTO patchDTO) 
        {
            await accuroObservationRepository.UpdateObservationGroupsUploads(patchDTO);

            if (!string.IsNullOrEmpty(patchDTO.Activity)) 
            {
                AccuroLabObservationResultsActivityDTO accuroLabObsResultsActivityDTO = new()
                {
                    PatientId = patchDTO.PatientId,
                    CollectionDate = patchDTO.CollectionDate,
                    Activity = patchDTO.Activity,
                    CreatedDate = DateTime.Now,
                    UserId = userContextService.TipsUserId
                };
                await accuroObsResultsActivityRepository.AddAccuroLabObservationResult(accuroLabObsResultsActivityDTO);
            }
        }

        public async Task<IEnumerable<AccuroLogDTO>> GetPatientUploadedLogs(int patientId) 
        {
            return await accuroObservationRepository.GetPatientUploadedLogs(patientId);
        }
    }
}
