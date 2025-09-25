using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Service.Logging;
using TestManager.Interfaces;
using TestManager.DataAccess.Helper;

namespace TestManager.Service
{
    public interface IAndrologistService
    {
        public Task<List<AndrologistDto>> GetAndrologistsAsync(AndrologistFilterDTO? filter = null);

        public Task<AndrologistDto> AddAndrologist(AndrologistDto andrologistDto);

        Task<AndrologistDto?> UpdateAndrologist(AndrologistDto andrologistDto);

        Task<bool> DeleteAdnrologist(int Id);
    }

    public class AndrologistService(IAndrologistRepository andrologistRepository,
        IActivityLogRepository activityLogRepository,
        IUserContextService userContextService) : IAndrologistService
    {
        public async Task<List<AndrologistDto>> GetAndrologistsAsync(AndrologistFilterDTO? filter = null)
        {
            var result = await andrologistRepository.GetAndrologistsAsync(filter);
            DomainEventLogger.LogDomainEvent("GetAndrologist", new Dictionary<string, object>
            {
                {"Action", "Get" },
                {"Andrologist Count", result.Count }
            });

            return result;
        }

        public async Task<AndrologistDto> AddAndrologist(AndrologistDto andrologistDto)
        {
            var  result = await andrologistRepository.AddAndrologist(andrologistDto);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Insert",
                EntityTypeId = 0,
                InstanceId = andrologistDto.AndrologistId,
                EntityAction = $"Add Andrologist: {andrologistDto.AndrologistId}, " +
                $" Andrologist Name: {andrologistDto.FirstName} {andrologistDto.LastName} Gender: {andrologistDto.Gender} " +
                $" Address: {andrologistDto.Address}",
                UserEmail = userContextService.Email ?? "Unknown"
            });

            DomainEventLogger.LogDomainEvent("AddAndrologist", new Dictionary<string, object>
            {
                { "AndroloogistId", result.AndrologistId },
                { "Action", "Insert" },
                { "AdnrologistName", andrologistDto.FirstName + " " + andrologistDto.LastName ?? "Unknown" }
            });
            return result;
        }

        public async Task<AndrologistDto?> UpdateAndrologist(AndrologistDto andrologistDto)
        {
            var result = await andrologistRepository.UpdateAndrologist(andrologistDto);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Update",
                EntityTypeId = 0,
                InstanceId = andrologistDto.AndrologistId,
                EntityAction = $"Update Andrologist: {andrologistDto.AndrologistId}, " +
                $" Andrologist Name: {andrologistDto.FirstName} {andrologistDto.LastName} Gender: {andrologistDto.Gender} " +
                $" Address: {andrologistDto.Address}",
                UserEmail = userContextService.Email ?? "Unknown"
            });

            DomainEventLogger.LogDomainEvent("UpdateAndrologist", new Dictionary<string, object>
            {
                { "AndroloogistId", andrologistDto.AndrologistId },
                { "Action", "Update" },
                { "AdnrologistName", andrologistDto.FirstName + " " + andrologistDto.LastName ?? "Unknown" }
            });
            return result;
        }

        public async Task<bool> DeleteAdnrologist(int id)
        {
            var result = await andrologistRepository.DeleteAdnrologist(id);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Delete",
                EntityTypeId = 0,
                InstanceId = id,
                EntityAction = $"Delete Andrologist with Id: {id} ",
                UserEmail = userContextService.Email ?? "Unknown"
            });

            DomainEventLogger.LogDomainEvent("DeleteAndrologist", new Dictionary<string, object>
            {
                { "AndrologistId", id },
                { "Action", "Delete" }               
            });
            return result;
        }
    }
}
