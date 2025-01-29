using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalVehicle.Models
{
    public class RentalTransaction
    {
        [Key]
        public int TransactionID { get; set; }

        // Foreign Key to RentalBooking
        [Required]
        [ForeignKey(nameof(RentalBooking))]
        public int BookingID { get; set; }

        // Foreign Key to VehicleReturn
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total Due Amount must be a positive value.")]
        public decimal ActualTotalDueAmount { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Remaining Amount must be a positive value.")]
        public decimal TotalRemaining { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Refunded Amount must be a positive value.")]
        public decimal TotalRefundedAmount { get; set; }
        
        public string? PaymentDetails { get; set; }

        [Required]
        public DateTime? TransactionDate { get; set; }
        public DateTime? UpdatedTransactionDate { get; set; }

        //navigation
        public VehicleReturn? VehicleReturn { get; set; }//one to one
        public RentalBooking? RentalBooking { get; set; }//one to one

    }
}
