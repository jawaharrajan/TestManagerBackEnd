using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using TestManager.Domain.Model.Uploader;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Uploader
{
    public class AdviceRepository(ApplicationDbContext _) : GenericRepository<Advice, int>(_), IAdviceRepository
    {
        public async Task<IEnumerable<AdviceDTO>> GetAdviceByPatientId(int patientId)
        {
            var result = await (from a in _context.Advice
                                where a.PatientId == patientId
                                select new AdviceDTO
                                {
                                    AdviceId = a.AdviceId,
                                    PatientId = a.PatientId,
                                    NurseCommunicationTypeId = a.NurseCommunicationTypeId,
                                    Text = a.Text,
                                    UserId = a.UserId,
                                    CreateDate = a.CreateDate,
                                    Result = a.Result,
                                    AppointmentId = a.AppointmentId
                                }).ToListAsync();

            return result;
        }

        public async Task<AdviceDTO> AddAdvice(AdviceDTO adviceDTO)
        {
            Advice advice = new()
            {
                PatientId = adviceDTO.PatientId,
                NurseCommunicationTypeId = adviceDTO.NurseCommunicationTypeId,
                Text = adviceDTO.Text,
                UserId = adviceDTO.UserId,
                CreateDate = adviceDTO.CreateDate,
                Result = adviceDTO.Result,
                AppointmentId = adviceDTO.AppointmentId
            };

            await AddAsync(advice);
            adviceDTO.AdviceId = advice.AdviceId;
            return adviceDTO;
        }
    }
}