namespace TestManager.Domain.DTO
{
    public class TransactionItemDto
    {
        public int TransactionItemId { get; set; }
        public string? Description { get; set; }

        public int? ProductId { get; set; }

        public string? Auxiliary { get; set; }

        public int? UserId { get; set; }
        public string? AccessionNo { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? LastUpdated { get; set; }

    }

}
