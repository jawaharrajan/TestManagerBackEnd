namespace TestManager.Domain.Model;

public partial class Doctor : BaseEntity<int>
{
    //public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Initials { get; set; }

    public byte? Gender { get; set; }

    public string? License { get; set; }

    public string? CPSO { get; set; }

    public byte? TrueDoctor { get; set; }

    public byte? ShowInInvoice { get; set; }

    public byte? DisplaySalutation { get; set; }

    public byte? SeeingNonResidents { get; set; }

    public string? DesignatedNurse { get; set; }

    public string? PlatinumNurse { get; set; }

    public string? ReferralCoordinator { get; set; }

    public string? Affiliation { get; set; }

    public string? ProviderType { get; set; }

    public string? OhipProvider { get; set; }

    public string? OhipGroup { get; set; }

    public int OhipSpecialtyCode { get; set; }
}
