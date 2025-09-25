namespace TestManager.Domain.Model;

public partial class Patient : BaseEntity<int>
{
    public int PatientId { get; set; }

    public string? Salutation { get; set; }

    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; } = null!;
    public string? FullName { get; set; }

    public string? Initials { get; set; }

    public string? Nickname { get; set; }

    public string? PronouncedAs { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Birthdate { get; set; }

    public string? Photofile { get; set; }

    public string? Healthcard { get; set; }

    public string? HealthcardVersion { get; set; }

    public byte? SpecialNeeds { get; set; }

    public string? Email { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public byte? NonResident { get; set; }

    public string? PrimaryAreaCode { get; set; }

    public string? PrimaryPhone { get; set; }

    public string? PrimaryExt { get; set; }

    public string? AlternateAreaCode { get; set; }

    public string? AlternatePhone { get; set; }

    public string? AlternateExt { get; set; }

    public string? InternationalPhone { get; set; }

    public string? FaxAreaCode { get; set; }

    public string? Fax { get; set; }

    public string? EmergencyName { get; set; }

    public string? EmergencyAreaCode { get; set; }

    public string? EmergencyPhone { get; set; }

    public string? EmergencyRelationship { get; set; }

    public string? Company { get; set; }

    public string? Title { get; set; }

    public string? BusinessAreaCode { get; set; }

    public string? BusinessPhone { get; set; }

    public string? BusinessExt { get; set; }

    public string? CellAreaCode { get; set; }

    public string? Cell { get; set; }

    public string? BusinessAddress01 { get; set; }

    public string? BusinessAddress02 { get; set; }

    public string? BusinessAddress03 { get; set; }

    public string? BusinesCity { get; set; }

    public string? BusinessProvince { get; set; }

    public string? BusinessPostalCode { get; set; }

    public string? BusinessCountry { get; set; }

    public string? Assistant { get; set; }

    public string? AssistantAreaCode { get; set; }

    public string? AssistantPhone { get; set; }

    public string? AssistantExtension { get; set; }

    public string? Pharmacy { get; set; }

    public string? PharmacyAreaCode { get; set; }

    public string? PharmacyPhone { get; set; }

    public string? FileNumber { get; set; }

    public byte? SendNews { get; set; }

    public byte? VIP { get; set; }

    public byte? BadExperience { get; set; }

    public DateOnly? PatientSince { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime? RecallDate { get; set; }

    public string? RecallNote { get; set; }

    public string? HSID { get; set; }

    public string? Suffix { get; set; }

    public byte? IsAfricanAmerican { get; set; }

    public string? ConfidentialEmail { get; set; }

    public byte? NoGeneralEmail { get; set; }

    public byte? NoConfidentialEmail { get; set; }

    public byte? DoNotEmail { get; set; }

    public string? OtherHealthNumber { get; set; }

    public byte? IsPotential { get; set; }

    public byte? IsEZFacilityOutgoing { get; set; }

    public byte? IsSentToEZFacility { get; set; }

    public byte? IsEZNeedToBeUpdated { get; set; }

    public bool? DoNotUploadTomytestclient { get; set; }

    public string? DoNotUploadTomytestclientId { get; set; }

    public byte? IsYouth { get; set; }

    public byte? IsRetiree { get; set; }

    public byte? IsInPACS { get; set; }

    public byte? Family_Doctor { get; set; }

    public string? Marital_Status { get; set; }

    public string? WorkPosition { get; set; }

    public string? Ethnicity { get; set; }

    public string? Grade { get; set; }

    public string? HealthCardEncrypt { get; set; }

    public string? HealthCardNumber { get; set; }

    public byte? IsHearWare { get; set; }

    public string? Language { get; set; }

    public string? LastAppointmentWithFamilyDoctor { get; set; }

    public string? MiddleName { get; set; }

    public int? NumberOfChildren { get; set; }

    public string? PrimaryPhoneType { get; set; }

    public string? SecondaryPhoneType { get; set; }

    public string? WorkStatus { get; set; }

    public string? WorkTitle { get; set; }

    public string? OldGeneralEmail { get; set; }

    public string? NoRecall { get; set; }

    public string? ManualFlagNoRecall { get; set; }

    public string? PreferredLanguage { get; set; }

    public byte? FirstTimePrepPack { get; set; }

    public string? AssistantEmail { get; set; }

    public int? AccuroId { get; set; }

    public byte? IsIDMClient { get; set; }

    public byte? AccessibleReport { get; set; }

    public string? PreferredPhone { get; set; }
    
    public bool? HasMytestclient { get; set; }

    //Navigation Property
    public  Appointment? Appointment { get; set; }
}
