namespace API.Entities;

public class BusLocation
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public double Latitude { get; set; }

     public double Longitude { get; set; }

    public Guid DiverId { get; set; }

    public string DriverName { get; set; } = string.Empty;
   
    public DateTime Timestamp { get; set; }
}
