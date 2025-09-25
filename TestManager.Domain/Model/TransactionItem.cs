namespace TestManager.Domain.Model;

public partial class TransactionItem : BaseEntity<int>
{
    public int TransactionItemId { get; set; }

    public int? TransactionId { get; set; }

    public string? Description { get; set; }

    public string? NonInvoiceContent { get; set; }

    public string? Auxiliary { get; set; }

    public int? ProductId { get; set; }

    public string? AccpacItem { get; set; }

    public int? AccountId { get; set; }

    public string? OhipCode01 { get; set; }

    public string? OhipCode02 { get; set; }

    public string? OhipCode03 { get; set; }

    public string? OhipDiagnosticCode01 { get; set; }

    public string? OhipDiagnosticCode02 { get; set; }

    public string? OhipDiagnosticCode03 { get; set; }

    public decimal? OhipFacilityFee { get; set; }

    public decimal? OhipProfessionalFee { get; set; }

    public int? OhipTypeId { get; set; }

    public byte? SubmittedToMOH { get; set; }

    public int? UserId { get; set; }

    public string? OhipReferringProvider { get; set; }

    public string? AccessionNo { get; set; }

    public string? PACSProcedureId { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? LastUpdated { get; set; }

    public bool IsDeleted { get; set; }

    public  int InvoiceID { get; set; }
    //Navigation Porperties
    public Transaction? Transaction { get; set; }

    public Product Product { get; set; }
}
