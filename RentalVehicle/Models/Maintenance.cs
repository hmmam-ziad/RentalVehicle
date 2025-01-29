using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalVehicle.Models
    {
        public class Maintenance
        {
            [Key]
            public int MaintenanceID { get; set; }

            [Required]
            [ForeignKey(nameof(Vehicle))]
            public int VehicleID { get; set; } 

            [Required]
            [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
            public string? Description { get; set; } 

            [Required]
            [DataType(DataType.Date)]
            public DateTime MaintenanceDate { get; set; } 

            [Required]
            [Range(0, double.MaxValue, ErrorMessage = "Cost must be a positive value.")]
            public decimal Cost { get; set; } 

            [Required]
            [StringLength(100, ErrorMessage = "Maintenance type cannot exceed 100 characters.")]
            public string? MaintenanceType { get; set; } 

            [Required]
            [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
            public string? Status { get; set; } 

            // Navigation property
            public Vehicle? Vehicle { get; set; } 
        }
    }
