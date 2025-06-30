namespace API.Entities;

public enum PaymentStatus 
{
    Pending,
    InProgress,
    Completed,
    Cancelled
}

public enum PaymentMethod 
{
    CreditCard,
    PayPal,
    BankTransfer,
    Cash
}

public class Payment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } // Foreign key to the user entity
    public decimal Amount { get; set; } // Amount of the payment
    public string PaymentMethod { get; set; } = string.Empty; // e.g., Credit Card, PayPal
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow; // Date and time of the payment
    public string Status { get; set; } = "Pending"; // e.g., Pending, Completed, Failed
    public string PaymentMethodDetails { get; set; } = string.Empty; // Details of the payment method used
    public string paymentStatus { get; set; } = "Pending"; // Current status of the payment
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp when the payment was created

}