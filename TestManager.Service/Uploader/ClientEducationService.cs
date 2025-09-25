using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Users;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.DTO.Users;
using TestManager.Interfaces;

namespace TestManager.Service.Uploader
{
    public interface IClientEducationService
    {
        Task<IEnumerable<PrepClientEducationDTO>> GetClientEducationByPatientId(int patientId);
        Task<IEnumerable<PrepClientEducationDTO>> UpsertClientEducationByPatientId(int patientId, IEnumerable<PrepClientEducationDTO> clientEducation);
    }
    public class ClientEducationService(IPrepClientEducationRepository clientEducationRepository, 
        IUserContextService userContextService,
        IUserRepository userRepository) : IClientEducationService
    {
        public async Task<IEnumerable<PrepClientEducationDTO>> GetClientEducationByPatientId(int patientId)
        {
            return await clientEducationRepository.GetClientEducationByPatientId(patientId);
        }
        public async Task<IEnumerable<PrepClientEducationDTO>> UpsertClientEducationByPatientId(int patientId, IEnumerable<PrepClientEducationDTO> clientEducation)
        {
            UserDTO userDTO = new()
            {
                Email = userContextService.Email,
                FirstName = userContextService.FirstName,
                LastName = userContextService.LastName
            };

            var userId = await userRepository.GetUserIdAsync(userDTO);
            return await clientEducationRepository.UpsertClientEducationByPatientId(patientId, clientEducation, userId);
        }
    }
    
}
