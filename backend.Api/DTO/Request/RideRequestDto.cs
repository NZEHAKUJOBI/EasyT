

using Swashbuckle.AspNetCore.Annotations;

namespace backend.API.DTO.Request
{
    public class RideRequestDto
    {
        [SwaggerIgnore]
        public Guid PassengerId { get; set; }

        public string PickupLocation { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }

        public string DropoffLocation { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }

        [SwaggerIgnore]
        public string RideStatus { get; set; } = "Requested"; // Default status
    }
}
