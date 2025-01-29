using System.ComponentModel.DataAnnotations;

namespace RentalVehicle.Models
{
    public class VehicleCategories
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        public string? CategoryName { get; set; }

        public List<Vehicle>? Vehicles { get; set; }
    }
}
