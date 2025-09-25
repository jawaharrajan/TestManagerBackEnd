using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model.Uploader;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Uploader
{
    public class PrepClientEducationRepository(ApplicationDbContext _) : GenericRepository<Prep_ClientEducation, int>(_), IPrepClientEducationRepository
    {
        public async Task<IEnumerable<PrepClientEducationDTO>> GetClientEducationByPatientId(int patientId) 
        {
            return await _context.Prep_ClientEducation.Where(ce => ce.PatientId == patientId).Select(x => new PrepClientEducationDTO()
            {
                PatientId = x.PatientId,
                ClientEducationId = x.ClientEducationId,
                AppointmentId = x.AppointmentId,
                CreateDate = x.CreateDate,
                EducationMaterialId = x.EducationMaterialId,
                InActive = x.InActive,
                InActiveByUserId = x.InActiveByUserId,
                InActiveDate = x.InActiveDate,
                UserId = x.UserId,
                Viewed = x.Viewed
            }).ToListAsync();
        }

        public async Task<IEnumerable<PrepClientEducationDTO>> UpsertClientEducationByPatientId(int patientId, IEnumerable<PrepClientEducationDTO> clientEducation, int userId)
        {
            var inputEducationDict = clientEducation.ToDictionary(x => x.EducationMaterialId);
            var existingEducationDict = await _context.Prep_ClientEducation.Where(ce => ce.PatientId == patientId).ToDictionaryAsync(x => x.EducationMaterialId);

            var educationToAdd = new List<Prep_ClientEducation>();
            foreach (var educationMaterialId in inputEducationDict.Keys)
            {
                var inputEducation = inputEducationDict[educationMaterialId];
                if (!existingEducationDict.ContainsKey(educationMaterialId))
                {
                    educationToAdd.Add(new Prep_ClientEducation()
                    {
                        AppointmentId = inputEducation.AppointmentId,
                        CreateDate = DateTime.Now,
                        EducationMaterialId = inputEducation.EducationMaterialId,
                        InActive = inputEducation.InActive,
                        PatientId = patientId,
                        UserId = userId
                    });
                }
                else 
                {
                    var educationRecord = existingEducationDict[educationMaterialId];
                    if (!educationRecord.Viewed && inputEducation.Viewed) 
                    {
                        educationRecord.Viewed = true;
                    }
                    if (!educationRecord.InActive && inputEducation.InActive)
                    {
                        educationRecord.InActive = true;
                        educationRecord.InActiveDate = DateTime.Now;
                        educationRecord.InActiveByUserId = userId;
                    }
                }
            }


            if (educationToAdd.Any())
            {
                _context.Prep_ClientEducation.AddRange(educationToAdd);
            }
            
            await _context.SaveChangesAsync();

            return educationToAdd.Concat(existingEducationDict.Values).Select(x => new PrepClientEducationDTO() 
            {
                PatientId = x.PatientId,
                ClientEducationId = x.ClientEducationId,
                AppointmentId = x.AppointmentId,
                CreateDate = x.CreateDate,
                EducationMaterialId = x.EducationMaterialId,
                InActive = x.InActive,
                InActiveByUserId = x.InActiveByUserId,
                InActiveDate = x.InActiveDate,
                UserId = x.UserId,
                Viewed = x.Viewed
            });
        }


    }
}
