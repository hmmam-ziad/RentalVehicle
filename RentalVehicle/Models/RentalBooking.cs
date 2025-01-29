using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalVehicle.Models
{
    public class RentalBooking
    {
        [Key]
        public int BookingID { get; set; }
        //Foreign Key to Customer
        [Required]
        [ForeignKey(nameof(Customer))]
        public int CustomerID { get; set; }
        //Foreign Key to Vehicle
        [Required]
        [ForeignKey(nameof(Vehicles))]
        public int VehicleID { get; set; }

        [Required]
        public DateTime RentalStartDate { get; set; }

        [Required]
        public DateTime RentalEndDate { get; set; }
        [Required]
        [StringLength(200)]
        public string? PickupLocation { get; set; }

        public int Rating { get; set; }

        [StringLength(200)]
        public string? DropoffLocation { get; set; }

        [Required]
        [ValidateNever]
        public int InitialRentalDays { get; set; }

        [Required]
        [ValidateNever]
        [Range(0, double.MaxValue, ErrorMessage = "The rental price must be a positive value.")]
        public decimal RentalPricePerDay { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The total due amount must be a positive value.")]
        public decimal? InitialTotalDueAmount { get; set; }
        [Required]
        [StringLength(500)]
        public string? InitialCheckNotes { get; set; }

        // navigation
        public Customer? Customer { get; set; }   // many to 1 with customer
        public Vehicle? Vehicles { get; set; }     // many to 1 with vehicle
        public RentalTransaction? RentalTransaction { get; set; } //1 to 1 with RentalTransactions

    }
}
