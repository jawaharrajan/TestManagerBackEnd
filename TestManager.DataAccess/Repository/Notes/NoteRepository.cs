using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TestManager.DataAccess.Repository.Notes
{
    public class NoteRepository(ApplicationDbContext _) : GenericRepository<Note, int>(_), INoteRepository
    {
        public async Task<List<NoteDTO>> GetNotes(int entityTypeId, int instanceId)
        {
            return await _context.Note
                .Where(n => n.EntityTypeID == entityTypeId)
                .Where(n => n.InstanceID == instanceId)
                .Select(n => new NoteDTO()
                {
                    CreateDate = n.CreateDate,
                    InstanceID = n.InstanceID,
                    EntityTypeID = n.EntityTypeID,
                    NoteID = n.NoteID,
                    NoteType = n.NoteType,
                    Text = n.Text,
                    UserID = n.UserID,
                    FirstName = n.User.FirstName,
                    LastName = n.User.LastName                    
                }).ToListAsync();
        }
    }
}
