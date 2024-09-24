namespace WebApi.Model
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsActive { get; set; }
        public decimal? PreviousPrice { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public required string ApprovalReason { get; set; }
        public DateTime? ApprovalRequestDate { get; set; }
        public ProductState State { get; set; } // states are Created, Updated, Deleted)
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum ApprovalStatus
    {
        PendingApproval,
        Approved,
        Rejected
    }

    public enum ProductState
    {
        Created,
        Updated,
        Deleted
    }
}
