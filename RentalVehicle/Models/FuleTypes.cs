using System.ComponentModel.DataAnnotations;

namespace RentalVehicle.Models
{
    public class FuleTypes
    {
        [Key]
        public int FuleID { get; set; }
        [Required]
        public string? FuleType { get; set; }

        public List<Vehicle>? Vehicles { get; set; }// one to many
    }
}
