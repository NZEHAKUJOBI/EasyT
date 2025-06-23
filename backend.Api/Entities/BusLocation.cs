namespace backend.Api.Entities;

public class BusLocation
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; }
}
