namespace backend.API.DTO.Request;
using Swashbuckle.AspNetCore.Annotations;

    public class BusLocationRequest
    {
        [SwaggerIgnore]
        public Guid DriverId { get; set; }
        [SwaggerIgnore]
        public Guid TripId { get; set; }
        [SwaggerIgnore]
        public Guid BusId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [SwaggerIgnore]
        public DateTime Timestamp { get; set; }

        public string LocationName { get; set; } = string.Empty;
        public string Status { get; set; } = "Active"; // Default status
        [SwaggerIgnore]
        public string DriverName { get; set; } = string.Empty;
    
    }
