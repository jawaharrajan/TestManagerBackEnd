namespace TestManager.Domain.Model
{
    public class Invoice : BaseEntity<int>
    {
        public int InvoiceID { get; set; }
        public int TransactionID { get; set; }
        public int Number { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int AccointID { get; set; }
        public int InvoiceTypeID { get; set; }
        public int RefToInvoiceID { get; set; }
        public bool Exported { get; set; }
        public DateTime ExportedDate { get; set; }
        public  bool IsOHIPInvoice { get; set; }

        //navigation
        public Transaction Transaction { get; set; }
    }
}
