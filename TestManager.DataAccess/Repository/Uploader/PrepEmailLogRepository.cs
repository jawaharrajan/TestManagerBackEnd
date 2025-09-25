using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using TestManager.Domain.Model.Uploader;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Uploader
{
    public class PrepEmailLogRepository(ApplicationDbContext _) : GenericRepository<PrepEmailLog, int>(_), IPrepEmailLogRepository
    {
        public async  Task<IEnumerable<PrepEmailLogDTO>> GetEmailLogsByPatientId(int patientId)
        {
            var result = await (from l in _context.PrepEmailLog
                                where l.PatientId == patientId
                                select new PrepEmailLogDTO
                                {
                                    PatientId = l.PatientId,
                                    UserId = l.UserId,
                                    CreateDate = l.CreateDate,
                                    EmailLogId = l.EmailLogId,
                                    
                                }).ToListAsync();
            return result;
        }
    }
}