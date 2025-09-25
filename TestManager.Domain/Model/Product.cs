namespace TestManager.Domain.Model;

public partial class Product : BaseEntity<int>
{
    public int ProductID { get; set; }

    public int? BaseProductId { get; set; }

    public int? ProductTypeId { get; set; }

    public int? ProductGroupId { get; set; }

    public string? AccpacItem { get; set; }

    public string? Name { get; set; } = null!;

    public int? AppointmentTypeId { get; set; }

    public int? CancelCutOff { get; set; }

    public DateTime? NewPriceEffectiveDate { get; set; }

    public int? DiscountTimeFrame { get; set; }

    public decimal? DiscountAmount { get; set; }

    public int? AccountId { get; set; }

    public bool? AccountPays { get; set; }

    public bool? AccountPaysCancellation { get; set; }

    public bool? FamilyCoverage { get; set; }

    public string? OhipCode01 { get; set; }

    public string? OhipCode02 { get; set; }

    public int? OhipTypeId { get; set; }

    public decimal? OhipFacilityFee { get; set; }

    public int? ReminderEmailTemplateId { get; set; }

    public string? ReminderEmailAttachment { get; set; }

    public int? PrepPackTemplateId { get; set; }

    public int? ConfirmationEmailTemplateId { get; set; }

    public string? ConfirmationEmailAttachment { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? LastUpdated { get; set; }

    public bool? ShowDiagnosticFields { get; set; }

    public bool? ShowReferringDoctor { get; set; }

    public bool? IsShowCHA5YearsPrice { get; set; }

    public int? ModalityId { get; set; }

    public int? ProductSubgroupId { get; set; }

    //public bool IsDeleted { get; set; }

    public decimal? OhipProfessionalFee { get; set; }

    public ICollection<TransactionItem> TransactionItems { get; set; } = [];
}
