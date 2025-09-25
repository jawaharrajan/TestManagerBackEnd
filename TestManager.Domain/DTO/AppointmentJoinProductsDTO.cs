namespace TestManager.Domain.DTO
{
    public class AppointmentJoinProductsDTO
    {
        public int AppointmentId { get; set; }
        public List<JoinProduct> TransactionItems { get; set; } = new();
    }

    public class JoinProduct
    {
        public int TransactionItemId { get; set; }
        public string ProductName { get; set; }
        public int AccessionNumber { get; set; }
    }
}
