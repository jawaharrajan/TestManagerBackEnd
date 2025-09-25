using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Service.Logging;
using TestManager.Interfaces;
using TestManager.DataAccess.Helper;

namespace TestManager.Service
{
    public interface IAppointmentRuleService
    {
        Task<List<AppointmentRuleDTO>> GetAllAppointmentRules();
        Task<AppointmentRuleDTO> AddAppointmentRule(AppointmentRuleDTO appointmentRuleDto);
        Task<AppointmentRuleDTO> UpdateAppointmentRule(AppointmentRuleDTO appointmentRuleDto);
        Task<bool> DeleteAppointmentRule(int id);
    }

    public class AppointmentRuleService(IAppointmentRuleRepository appointmentRuleRepository,
        IActivityLogRepository activityLogRepository,
        IUserContextService userContextService) : IAppointmentRuleService
    {
        public async Task<List<AppointmentRuleDTO>> GetAllAppointmentRules()
        {
            var result = await appointmentRuleRepository.GetAllAppointmentRules();
            DomainEventLogger.LogDomainEvent("GetAppointmentRules", new Dictionary<string, object>
            {
                {"Action", "Get" },
                {"AppointmentRules Count", result.Count }
            });
            return result;
        }

        public async Task<AppointmentRuleDTO> AddAppointmentRule(AppointmentRuleDTO appointmentRuleDto)
        {
            var result = await appointmentRuleRepository.AddAppointmentRule(appointmentRuleDto);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Insert",
                EntityTypeId = 0,
                InstanceId = appointmentRuleDto.Id,
                EntityAction = $"Add AppointmentRule: {appointmentRuleDto.Id}, " +
                $" Product ID: {appointmentRuleDto.ProductId} Appointment TypeId: {appointmentRuleDto.AppointmentTypeId} Is Ative: {appointmentRuleDto.IsActive} " +
                $" Is Female: {appointmentRuleDto.IsFemale} Is Male: {appointmentRuleDto.IsMale} Age From: {appointmentRuleDto.AgeFrom} Age To: {appointmentRuleDto.AgeFrom} " +
                $" AddDaysToAge: {appointmentRuleDto.AddDaysToAge}",
                UserEmail = userContextService.Email ?? "Unknown"
            });

            DomainEventLogger.LogDomainEvent("AddAppointmentRules", new Dictionary<string, object>
            {
                {"AppointmentRulesId", result.Id },
                {"Action", "Insert" },
                {"AppointmentTypeId",result.AppointmentTypeId },
                {"ProductId", result.ProductId }                
            });

            return result;
        }

        public async Task<AppointmentRuleDTO> UpdateAppointmentRule(AppointmentRuleDTO appointmentRuleDto)
        {
            var result = await appointmentRuleRepository.UpdateAppointmentRule(appointmentRuleDto);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Update",
                EntityTypeId = 0,
                InstanceId = appointmentRuleDto.Id,
                EntityAction = $"UPdate AppointmentRule: {appointmentRuleDto.Id}, " +
                $" Product ID: {appointmentRuleDto.ProductId} Appointment TypeId: {appointmentRuleDto.AppointmentTypeId} Is Ative: {appointmentRuleDto.IsActive} " +
                $" Is Female: {appointmentRuleDto.IsFemale} Is Male: {appointmentRuleDto.IsMale} Age From: {appointmentRuleDto.AgeFrom} Age To: {appointmentRuleDto.AgeFrom} " +
                $" AddDaysToAge: {appointmentRuleDto.AddDaysToAge}",
                UserEmail = userContextService.Email ?? "Unknown"
            });

            DomainEventLogger.LogDomainEvent("UpdateAppointmentRules", new Dictionary<string, object>
            {
                {"AppointmentRulesId", result.Id },
                {"Action", "Update" },
                {"AppointmentTypeId",result.AppointmentTypeId },
                {"ProductId", result.ProductId }
            });

            return result;
        }

        public async Task<bool> DeleteAppointmentRule(int id)
        {
            var result = await appointmentRuleRepository.DeleteAppointmentRule(id);

            DateTime estDate = DateTimeConverter.ConvertTimeToRequiredTimeZone("EST");

            await activityLogRepository.AddAsync(new Domain.Model.ActivityLog
            {
                ActivityDate = estDate,
                SQLAction = "Delete",
                EntityTypeId = 0,
                InstanceId = id,
                EntityAction = $"Delete AppointmentRule with Id: {id} ",
                UserEmail = userContextService.Email ?? "Unknown"
            });

            DomainEventLogger.LogDomainEvent("DeleteAppointmentRules", new Dictionary<string, object>
            {
                {"AppointmentRulesId", id },
                {"Action", "Delete" }
            });

            return result;
        }


    }
}
