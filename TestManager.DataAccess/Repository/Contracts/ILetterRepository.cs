using TestManager.Domain.DTO.Uploader;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface ILetterRepository 
    {
        Task<int> AddLetter(PrepLetterDTO prepLetterDTO);
        Task<PrepLetterDTO?> GetLetterById(int letterId);
        Task RemoveLetter(int letterId);
        Task<IEnumerable<PrepLetterDTO>> GetLettersByPatientIdLetterTypeId(int patientId, int letterTypeId);

    }
}
