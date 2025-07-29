
using System;

namespace API.Entities
{
    public class RideRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public User Passenger { get; set; }

        public Guid TenantId { get; set; }
        public Tenant TenantName { get; set; }
        public Guid? DriverId { get; set; }
        public User Driver { get; set; }
        public string PickupLocation { get; set; }

        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public string DropoffLocation { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }
        public double DistanceInKm { get; set; }
        public decimal Fare { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public RideStatus Status { get; set; } = RideStatus.Requested;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public enum RideStatus
    {
        Requested,
        Accepted,
        OnTrip,
        Completed,
        Cancelled
    }
}
