using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Service
{

    public interface IAppointmentStatusService
    {
        public Task<List<AppointmentStatusDTO>> GetAppointmentStatusAsync();
    }
    public class AppointmentStatusService(IAppointmentStatusRepository appointmentStatusRepository) : IAppointmentStatusService
    {
        private readonly IAppointmentStatusRepository _appointmentStatusRepository = appointmentStatusRepository;

        public async Task<List<AppointmentStatusDTO>> GetAppointmentStatusAsync()
        {
            var statues = await _appointmentStatusRepository.GetAppointmentStatusAsync();
            return statues;
        }
    }
}
