using System.ComponentModel.DataAnnotations;

namespace RentalVehicle.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        public string? CustomerName { get; set; }
        [Required]
        public string? ContactInformation { get; set; }
        [Required]
        
        public int? DriverLicenseNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]//email validation
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]//password validation
        public string? Password { get; set; }
        public string? Role { get; set; } //admin or user

        public List<RentalBooking>? RentalBooking { get; set; }//many to one
    }
}
