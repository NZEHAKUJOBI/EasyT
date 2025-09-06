using System;


namespace API.Entities
{
    public class GeneralNotification
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? DriverId { get; set; }   // Nullable if it's a broadcast
        public string Message { get; set; }
        public string Type { get; set; }      // e.g. "RideRequest", "SystemAlert"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public bool IsSent { get; set; } = false;
    }
}
