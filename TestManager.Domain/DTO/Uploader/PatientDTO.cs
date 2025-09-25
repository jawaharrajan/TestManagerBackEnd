using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Domain.DTO.Uploader
{
    public class PatientDTO
    {
        public int PatientId { get; set; }
        public string? Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhotoFile { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string? PrimaryAreaCode { get; set; }
        public string? PrimaryPhone { get; set; }
        public string? PrimaryExt { get; set; }
        public string? AlternateAreaCode { get; set; }
        public string? AlternatePhone { get; set; }
        public string? AlternateExt { get; set; }
        public string? InternationalPhone { get; set; }
        public string? FaxAreaCode { get; set; }
        public string? Fax { get; set; }
        public string? BusinessAreaCode { get; set; }
        public string? BusinessPhone { get; set; }
        public string? BusinessExt { get; set; }
        public string? CellAreaCode { get; set; }
        public string? Cell { get; set; }
        public string ConfidentialEmail { get; set; }
        public string? Healthcard { get; set; }

        public string? HealthcardVersion { get; set; }
        public bool? HasMytestclient { get; set; }
        public IEnumerable<PatientAppointmentsDTO> Appointments { get; set; }
        public int? AccuroId { get;  set; }
        public bool? DoNotUploadTomytestclient { get; set; }
        
    }
}
