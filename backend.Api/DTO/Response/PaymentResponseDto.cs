namespace backend.API.DTO.Response
{
    public class PaymentResponse
    {
        public string TransactionId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}