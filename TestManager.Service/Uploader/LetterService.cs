using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Users;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.DTO.Users;
using TestManager.Domain.Model;
using TestManager.Interfaces;
using System.Threading.Tasks;

namespace TestManager.Service.Uploader
{
    public interface ILetterService
    {
        Task<int> AddLetter(PrepLetterDTO prepLetterDTO);
        Task<PrepLetterDTO?> GetLetterById(int letterId);
        Task RemoveLetter(int letterId);
        Task<IEnumerable<PrepLetterDTO>> GetLettersByPatientIdLetterTypeId(int patientId, int letterTypeId);
    }

    public class LetterService(ILetterRepository letterRepository,
        IUserRepository userRepository,
        IUserContextService userContextService,
        IAdviceRepository adviceRepository) : ILetterService
    {
        public async Task<int> AddLetter(PrepLetterDTO prepLetterDTO)
        {
            UserDTO userDTO = new()
            {
                Email = userContextService.Email,
                FirstName = userContextService.FirstName,
                LastName = userContextService.LastName
            };

            prepLetterDTO.UserId = await userRepository.GetUserIdAsync(userDTO);
            if (prepLetterDTO.Attachments != null) 
            {
                foreach (var attachment in prepLetterDTO.Attachments) 
                {
                    attachment.UserId = prepLetterDTO.UserId;
                }
            }
            int result = await letterRepository.AddLetter(prepLetterDTO);
            if (result > 0)
            {
                AdviceDTO adviceDTO = new()
                {
                    PatientId = prepLetterDTO.PatientId,
                    Text = prepLetterDTO.Body,
                    AppointmentId = prepLetterDTO.AppointmentId,
                    UserId = prepLetterDTO.UserId,
                    CreateDate = DateTime.Now,
                    NurseCommunicationTypeId = prepLetterDTO.NurseCommunicationTypeId

                };
                if (prepLetterDTO.Attachments?.Any() == true) 
                {
                    adviceDTO.Text += $"<br/><strong class=\"text-xs\">Attached File: {prepLetterDTO.Attachments.ElementAt(0).Filename}</strong>";
                }
                await adviceRepository.AddAdvice(adviceDTO);
                return result;
            }
            else
                return 0;
        }
        public async Task<PrepLetterDTO?> GetLetterById(int letterId)
        {
            return await letterRepository.GetLetterById(letterId);
        }
        
        public async Task RemoveLetter(int letterId)
        {
            await letterRepository.RemoveLetter(letterId);
        }

        public Task<IEnumerable<PrepLetterDTO>> GetLettersByPatientIdLetterTypeId(int patientId, int letterTypeId)
        {
            return letterRepository.GetLettersByPatientIdLetterTypeId(patientId,letterTypeId);
        }
    }
}
