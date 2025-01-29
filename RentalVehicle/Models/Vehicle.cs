using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalVehicle.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }

        [Required]
        [ForeignKey(nameof(FuleType))]
        public int FuleTypeID { get; set; }

        [Required]
        [ForeignKey(nameof(VehicleCategories))]
        public int CarCategoryID { get; set; }

        
        [Required]
        [StringLength(100, ErrorMessage = "Make name can't be longer than 100 characters.")]
        public string? Make { get; set; }  // الشركة المصنعة

        [Required]
        [StringLength(100, ErrorMessage = "Model name can't be longer than 100 characters.")]
        public string? Model { get; set; }  // موديل السيارة

        [Required]
        public DateTime Year { get; set; }  // سنة الصنع

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Mileage must be a positive number.")]
        public int Mileage { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Plate number must be a positive number.")]
        public int PlateNumber { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rental price per day must be a positive value.")]
        public decimal RentalPricePerDay { get; set; }

        [Required]
        public bool ISAvailabelForRent { get; set; }  

        
        [StringLength(255, ErrorMessage = "Image file name can't be longer than 255 characters.")]
        public string? Image { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Transmission type can't be longer than 50 characters.")]
        public string? TransmissionType { get; set; } 

        [Required]
        [Range(1, 100, ErrorMessage = "Seats count must be between 1 and 100.")]
        public int SeatsCount { get; set; } 

        //Navigation Properties
        public List<Maintenance>? Maintenances { get; set; } // many to one
        public VehicleCategories? VehicleCategories { get; set; } // one to many
        public FuleTypes? FuleType { get; set; } // one to many
    }
}