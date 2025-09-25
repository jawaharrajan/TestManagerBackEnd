using TestManager.Domain.DTO;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface IAndrologistRepository : IGenericRepository<Andrologist, int>
    {
        Task<List<AndrologistDto>> GetAndrologistsAsync(AndrologistFilterDTO? andrologistFilterDTO = null);

        Task<AndrologistDto> AddAndrologist(AndrologistDto andrologistDto);

        Task<AndrologistDto?> UpdateAndrologist(AndrologistDto andrologistDto);

        Task<bool> DeleteAdnrologist(int Id);
    }
}
