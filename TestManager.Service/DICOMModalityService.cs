using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using Model = TestManager.Domain.Model;
using TestManager.Interfaces;
using TestManager.Service.Logging;
using TestManager.DataAccess.Helper;

namespace TestManager.Service
{
    public interface IDICOMModalityService
    {
        Task<List<DICOMModalityDTO>> GetDICOMModalityAsync(DICOMModalityFilterDTO? filter = null);

        Task<DICOMModalityDTO> AddDICOMModality(DICOMModalityDTO dicomModalityDTO);

        Task<DICOMModalityDTO?> UpdateDICOMModality(DICOMModalityDTO dicomModalityDTO);
        Task<bool> DeleteDICOMModality(int Id);
    }


    public class DICOMModalityService(IDICOMModalityRepository dicomModalityRepository,
        IActivityLogRepository activityLogRepository,
        IUserContextService userContextService) : IDICOMModalityService
    {
        public async Task<List<DICOMModalityDTO>> GetDICOMModalityAsync(DICOMModalityFilterDTO? filter = null)
        {
            var result = await dicomModalityRepository.GetDICOMModalityAsync(filter);
            DomainEventLogger.LogDomainEvent("GetDicommModality", new Dictionary<string, object>
            {
                {"Action", "Get" },
                {"DicommModality Count", result.Count }
            });

            return result;
        }

        public async Task<DICOMModalityDTO> AddDICOMModality(DICOMModalityDTO dicomModalityDTO)
        {
            var result = await dicomModalityRepository.AddDICOMModality(dicomModalityDTO);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Insert",
                EntityTypeId = 0,
                InstanceId = dicomModalityDTO.ModalityId,
                EntityAction = $"Add Dicommadility: {dicomModalityDTO.ModalityId}, " +
                $" Modality Code: {dicomModalityDTO.ModalityCode} Modality Code: {dicomModalityDTO.ModalityCode} Procedure Code: {dicomModalityDTO.ProcedureCode}",
                UserEmail = userContextService.Email ?? "Unknown"
            });

            DomainEventLogger.LogDomainEvent("AddDicommModality", new Dictionary<string, object>
            {
                {"DicommModalityId", result.ModalityId },
                {"Action", "Insert" },
                {"Prodcdure Code", result.ProcedureCode },
                {"Modality Code", result.ModalityCode }
            });

            return result;
        }

        public async  Task<DICOMModalityDTO?> UpdateDICOMModality(DICOMModalityDTO dicomModalityDTO)
        {
            var result =  await dicomModalityRepository.UpdateDICOMModality(dicomModalityDTO);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Update",
                EntityTypeId = 0,
                InstanceId = dicomModalityDTO.ModalityId,
                EntityAction = $"Update Dicommadility: {dicomModalityDTO.ModalityId}, " +
                $" Modality Code: {dicomModalityDTO.ModalityCode} Modality Code: {dicomModalityDTO.ModalityCode} Procedure Code: {dicomModalityDTO.ProcedureCode}",
                UserEmail = userContextService.Email ?? "Unknown"
            });

            DomainEventLogger.LogDomainEvent("UpdateDicommModality", new Dictionary<string, object>
            {
                {"DicommModalityId", result.ModalityId },
                {"Action", "Update" },
                {"Prodcdure Code", result.ProcedureCode },
                {"Modality Code", result.ModalityCode }
            });

            return result;
        }

        public async Task<bool> DeleteDICOMModality(int id)
        {
            var result = await dicomModalityRepository.DeleteDICOMModality(id);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Delete",
                EntityTypeId = 0,
                InstanceId = id,
                EntityAction = $"Delete DicomModality with Id: {id} ",
                UserEmail = userContextService.Email ?? "Unknown"
            });

            DomainEventLogger.LogDomainEvent("DeleteDicommModality", new Dictionary<string, object>
            {
                {"DicommModalityId", id },
                {"Action", "Delete" }
            });

            return result;
        }
    }
}
