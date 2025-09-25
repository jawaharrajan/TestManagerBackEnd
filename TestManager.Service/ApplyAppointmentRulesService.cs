using TestManager.DataAccess.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Service
{
    public interface IApplyAppointmentRulesService
    {
        Task<bool> RunAppointmentRules();
    }

    public class ApplyAppointmentRulesService(IApplyAppointmetRules applyAppointmetRules) : IApplyAppointmentRulesService
    {
        public Task<bool> RunAppointmentRules()
        {
            return applyAppointmetRules.RunAppointmentRules();
        }
    }
}
