namespace backend.API.Entities;
    public class PaymentHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } 
    }
