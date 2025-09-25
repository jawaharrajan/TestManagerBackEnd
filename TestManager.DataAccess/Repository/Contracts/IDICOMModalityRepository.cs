using TestManager.Domain.DTO;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IDICOMModalityRepository : IGenericRepository<DICOMModality, int>
    {
        Task<List<DICOMModalityDTO>> GetDICOMModalityAsync(DICOMModalityFilterDTO? filter = null);

        Task<DICOMModalityDTO> AddDICOMModality(DICOMModalityDTO dICOMModalityDTO);

        Task<DICOMModalityDTO?> UpdateDICOMModality(DICOMModalityDTO dICOMModalityDTO);

        Task<bool> DeleteDICOMModality(int Id);
    }
}
