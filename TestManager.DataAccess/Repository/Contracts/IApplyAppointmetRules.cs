using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IApplyAppointmetRules
    {
        Task<bool> RunAppointmentRules();
    }
}
