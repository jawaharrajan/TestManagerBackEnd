using TestManager.Domain.DTO;
using TestManager.Domain.Model;

namespace TestManager.DataAccess.Repository.Contracts
{
    public interface INoteRepository : IGenericRepository<Note, int>
    {
        Task<List<NoteDTO>> GetNotes(int entityTypeId, int instanceId);
    }
}
