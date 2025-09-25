using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO.TopLevelFilter;

namespace TestManager.Service.TopLevelFilter
{
    public interface ITopLevelDataService
    {
        Task<TopLevelDataDTO> GetTopLevelData();
    }

    public class TopLevelDataService(ILocationRepository locationRepository, 
        IAppointmentTypeRepository appointmentTypeRepository) : ITopLevelDataService
    {
        public async Task<TopLevelDataDTO> GetTopLevelData()
        {
            return new TopLevelDataDTO
            {
                AppointmentType = await appointmentTypeRepository.GetAppointmentType(),
                Location = await locationRepository.GetLocation()
            };            
        }
    }
}
