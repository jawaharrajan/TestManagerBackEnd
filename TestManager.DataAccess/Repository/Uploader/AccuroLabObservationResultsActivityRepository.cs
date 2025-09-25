using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using TestManager.Domain.Model.Uploader;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Uploader
{
    public class AccuroLabObservationResultsActivityRepository(ApplicationDbContext _) : GenericRepository<AccuroLabObservationResultsActivity, int>(_), IAccuroObservationResultsActivityRepository
    {
        public async Task<int> AddAccuroLabObservationResult(AccuroLabObservationResultsActivityDTO accuroLabObsResultsActivityDTO)
        {
            AccuroLabObservationResultsActivity accuroLabObsResultsActivity = new()
            {                
                PatientId = accuroLabObsResultsActivityDTO.PatientId,
                CollectionDate = accuroLabObsResultsActivityDTO.CollectionDate,
                Activity = accuroLabObsResultsActivityDTO.Activity,
                CreatedDate = DateTime.Now,
                UserId = accuroLabObsResultsActivityDTO.UserId
            };

            await AddAsync(accuroLabObsResultsActivity);
            return accuroLabObsResultsActivity.ObservationResultsLogId;
        }

        public async Task<IEnumerable<AccuroLabObservationResultsActivityDTO>> GetAccuroLabObsResultsActivityLogsByPatientId(int patientId)
        {
            var result = await (from a in _context.AccuroLabObservationResultsActivity
                                join u in _context.User on a.UserId equals u.UserId
                                where a.PatientId == patientId
                                select new AccuroLabObservationResultsActivityDTO
                                {
                                    ObservationResultsLogId = a.ObservationResultsLogId,
                                    PatientId =a.PatientId,
                                    CollectionDate = a.CollectionDate,
                                    Activity = a.Activity,
                                    CreatedDate = a.CreatedDate,
                                    UserId  = a.UserId,
                                    User = u != null ? u.FirstName + " " + u.LastName : null
                                }).ToListAsync();

            return result;
        }
    }
}
