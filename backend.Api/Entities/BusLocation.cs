namespace API.Entities;

using Swashbuckle.AspNetCore.Annotations;

public class BusLocation
{

    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string LocationName { get; set; } = string.Empty;
    [SwaggerIgnore]
    public string Status { get; set; }  // Default status

    public Guid DriverId { get; set; }

    public string DriverName { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }
}
