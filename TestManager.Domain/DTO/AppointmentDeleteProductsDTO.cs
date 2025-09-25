namespace TestManager.Domain.DTO
{
    public class AppointmentDeleteProductsDTO
    {
        public int AppointmentId { get; set; }
        public List<DeleteProduct> TransactionItems { get; set; } = new();
    }

    public class DeleteProduct
    {
        public int TransactionItemId { get; set; }
        public string ProductName { get; set; }
        public int? AccessionNumber { get; set; }
    }
}
