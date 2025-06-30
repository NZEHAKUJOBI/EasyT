namespace API.Entities;

public class Trip
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public Guid? PassengerId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Status { get; set; } = "Created"; // Created, Ongoing, Completed
    public double? DistanceKm { get; set; }
}
