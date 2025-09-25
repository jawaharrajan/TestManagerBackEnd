using TestManager.Domain.DTO;
using TestManager.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IAppointmentRuleRepository : IGenericRepository<AppointmentRule, int>
    {
        Task<List<AppointmentRuleDTO>> GetAllAppointmentRules();
        Task<AppointmentRuleDTO> AddAppointmentRule(AppointmentRuleDTO appointmentRuleDto);
        Task<AppointmentRuleDTO> UpdateAppointmentRule(AppointmentRuleDTO appointmentRuleDto);
        Task<bool> DeleteAppointmentRule(int id);

    }
}
