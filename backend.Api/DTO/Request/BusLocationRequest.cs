namespace backend.API.DTO.Request
{
    public class BusLocationRequest
    {
        public Guid BusId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }

        public string DriverName { get; set; } = string.Empty;
        public Guid DriverId { get; set; }
    }
}
