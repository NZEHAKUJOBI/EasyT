namespace API.Entities;

public class BusLocation
{
   
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public double Latitude { get; set; }

     public double Longitude { get; set; }

    public string LocationName { get; set; } = string.Empty;

    public string Status { get; set; } = "Active"; // Default status

    public Guid DriverId { get; set; }

    public string DriverName { get; set; } = string.Empty;
   
    public DateTime Timestamp { get; set; }
}
