namespace TestManager.Domain.Model;

public partial class Prm_PaedsConsent
{
    public int? ConsentId { get; set; }

    public int? AppointmentId { get; set; }

    public byte? ConsentExist { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? LiveWithName1 { get; set; }

    public string? LiveWithRelationship1 { get; set; }

    public string? LiveWithName2 { get; set; }

    public string? LiveWithRelationship2 { get; set; }

    public string? LiveWithName3 { get; set; }

    public string? LiveWithRelationship3 { get; set; }

    public string? LiveWithName4 { get; set; }

    public string? LiveWithRelationship4 { get; set; }

    public byte? DoesNotConsentToReleaseInfo { get; set; }

    public string? ShareInfoName1 { get; set; }

    public string? ShareInfoRelationship1 { get; set; }

    public string? ShareInfoName2 { get; set; }

    public string? ShareInfoRelationship2 { get; set; }

    public string? ShareInfoName3 { get; set; }

    public string? ShareInfoRelationship3 { get; set; }

    public string? ShareInfoName4 { get; set; }

    public string? ShareInfoRelationship4 { get; set; }
}
