

namespace backend.API.DTO.Request
{
    public class RideRequestDto
    {
        public Guid PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        
        public string PickupLocation { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }

        public string DropoffLocation { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }
    }
}
