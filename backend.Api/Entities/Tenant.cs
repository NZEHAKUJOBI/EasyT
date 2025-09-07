using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Tenant
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid DriverId { get; set; } // Reference to the driver associated with this tenant

        // Fleet owner or company name
        public string FirstName { get; set; }

        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;



        // Contact email for the tenant admin
        public string Email { get; set; }

        // Admin password for tenant management
        public string AdminPassword { get; set; }

        // Phone contact for administrative purposes
        public string PhoneNumber { get; set; }

        // Configuration specific to this tenant
        public decimal BaseFare { get; set; } = 500; // e.g., Naira base fare
        public decimal PerKilometerRate { get; set; } = 100; // e.g., Naira per km
        public decimal CommissionRate { get; set; } = 0.10m; // 10% commission

        // Soft delete flag if needed
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string Role  { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; } = false; // Default to false
        // Navigation properties (optional for multi-tenancy)
        public ICollection<User> Users { get; set; }
        public ICollection<RideRequest> RideRequests { get; set; }
    }
}
