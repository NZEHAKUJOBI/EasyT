namespace backend.API.Entities
{
    public class UserLocation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}