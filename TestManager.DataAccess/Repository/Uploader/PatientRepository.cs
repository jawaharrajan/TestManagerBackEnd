using TestManager.DataAccess.Repository.Contracts;
using TestManager.DataAccess.Repository.Radiology;
using TestManager.Domain.DTO.Uploader;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Uploader
{
    public class PatientRepository(ApplicationDbContext _) : GenericRepository<Patient, int>(_), IPatientRepository
    {
        public async Task<(List<PatientDTO> Patients, int TotalCount)> GetPatientsAsync(PatientFilterDTO? filter = null)
        {

            var query = from p in _context.Patient
                        orderby p.LastName ascending
                        select new PatientDTO
                        {
                            PatientId = p.PatientId,
                            FirstName = p.FirstName ?? string.Empty,
                            LastName = p.LastName ?? string.Empty,
                            Gender = p.Gender ?? string.Empty,
                            PhotoFile = p.Photofile ?? string.Empty,
                            Address1 = p.Address1 ?? string.Empty,
                            Address2 = p.Address2 ?? string.Empty,
                            City = p.City ?? string.Empty,
                            Province = p.Province ?? string.Empty,
                            Country = p.Country ?? string.Empty,
                            Email = p.Email ?? string.Empty,
                            PostalCode = p.PostalCode ?? string.Empty,
                            HasMytestclient = p.HasMytestclient == true,
                            AccuroId = p.AccuroId,
                            DoNotUploadTomytestclient = p.DoNotUploadTomytestclient == true
                        };

            #region - check for Filters
            if (filter != null)
            {                             
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(p =>
                        p.FirstName.Contains(filter.SearchTerm) ||
                        p.LastName.Contains(filter.SearchTerm));
                }
            }
            #endregion

            var totalCount = query.Count();

            var result = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

            return (result, totalCount);
        }

        public async Task<PatientDTO?> GetPatientByIdAsync(int patientId)
        {
            //var pat = await _context.Patient.Where(p => p.PatientId == patientId)
            //        .Select(p => new {
            //            p.PatientId,
            //            p.FirstName,
            //            p.LastName,
            //            // add more fields one by one to find which one fails
            //        }).FirstOrDefaultAsync();             

            PatientDTO? patient =  await _context.Patient.Where(p => p.PatientId == patientId).Select(p => new PatientDTO
            {
                PatientId = p.PatientId,
                Salutation = p.Salutation,
                FirstName = p.FirstName ?? string.Empty,
                LastName = p.LastName ?? string.Empty,
                Gender = p.Gender ?? string.Empty,
                PhotoFile = p.Photofile ?? string.Empty,
                Address1 = p.Address1 ?? string.Empty,
                Address2 = p.Address2 ?? string.Empty,
                City = p.City ?? string.Empty,
                Province = p.Province ?? string.Empty,
                Country = p.Country ?? string.Empty,
                Email = p.Email ?? string.Empty,
                PostalCode = p.PostalCode ?? string.Empty,
                ConfidentialEmail = p.ConfidentialEmail ?? string.Empty,
                AlternateAreaCode = p.AlternateAreaCode ?? string.Empty,
                AlternateExt = p.AlternateExt ?? string.Empty,
                AlternatePhone = p.AlternatePhone ?? string.Empty,
                Fax = p.Fax ?? string.Empty,
                FaxAreaCode = p.FaxAreaCode ?? string.Empty,
                InternationalPhone = p.InternationalPhone ?? string.Empty,
                PrimaryAreaCode = p.PrimaryAreaCode ?? string.Empty,
                PrimaryExt = p.PrimaryExt ?? string.Empty,
                PrimaryPhone = p.PrimaryPhone ?? string.Empty,
                BusinessAreaCode = p.BusinessAreaCode ?? string.Empty,
                BusinessExt = p.BusinessExt ?? string.Empty,
                BusinessPhone = p.BusinessPhone ?? string.Empty,
                Cell = p.Cell ?? string.Empty,
                CellAreaCode = p.CellAreaCode ?? string.Empty,
                Healthcard = p.Healthcard ?? string.Empty,
                HealthcardVersion = p.HealthcardVersion ?? string.Empty,
                AccuroId = p.AccuroId,
                HasMytestclient = p.HasMytestclient == true,
                DoNotUploadTomytestclient = p.DoNotUploadTomytestclient == true
            }).FirstOrDefaultAsync();

            if (patient == null) 
            {
                return null;
            }

            var dateThreshold = DateTime.Today.AddMonths(-18);
            patient.Appointments = await (from a in _context.Appointment
                                          join at in _context.AppointmentType on a.AppointmentTypeId equals at.Id
                                          where a.PatientId == patientId && a.Date > dateThreshold
                                          orderby a.Date descending
                                          select new PatientAppointmentsDTO()
                                          {
                                              Date = a.Date,
                                              AppointmentType = at.Name ?? string.Empty,
                                              Id = a.Id
                                          }).ToListAsync();

            return patient;
        }
    }
}
