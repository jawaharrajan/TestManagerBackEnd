using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model.Uploader;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IPrepClientEducationRepository : IGenericRepository<Prep_ClientEducation, int>
    {
        Task<IEnumerable<PrepClientEducationDTO>> GetClientEducationByPatientId(int id);
        Task<IEnumerable<PrepClientEducationDTO>> UpsertClientEducationByPatientId(int patientId, IEnumerable<PrepClientEducationDTO> clientEducation, int userId);
    }
}
