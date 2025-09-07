namespace backend.API.DTO.Request
{
    public class PaymentRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string Status { get; set; } 
    }
}