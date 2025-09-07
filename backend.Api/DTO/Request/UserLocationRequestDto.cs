namespace backend.API.Entities
{
    public class UserLocationRequestDto
    {
        public Guid Id { get; set; }
    
        public Guid UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}   