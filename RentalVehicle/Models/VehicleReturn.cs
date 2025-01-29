using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalVehicle.Models
{
    public class VehicleReturn
    {
        [Key]
        public int ReturnID { get; set; }

        [ForeignKey(nameof(RentalTransaction))]
        public int TransactionID { get; set; }

        // تاريخ العودة الفعلي
        [Required]
        public DateTime ActualReturnDate { get; set; }

        // عدد الأيام الفعلية المستأجرة
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Actual rental days must be at least 1 day.")]
        public int ActualRentalDays { get; set; }

        // قراءة عداد المسافات عند العودة
        [Range(0, int.MaxValue, ErrorMessage = "Mileage cannot be negative.")]
        public int Mileage { get; set; }

        // المسافة المستهلكة (وهي المسافة التي تم استخدامها فعلاً خلال فترة الإيجار)
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Consumed mileage cannot be negative.")]
        public int ConsumedMileage { get; set; }

        // ملاحظات الفحص النهائي
        [Required]
        [StringLength(500, ErrorMessage = "Final check notes cannot exceed 500 characters.")]
        public string? FinalCheckNotes { get; set; }

        // الرسوم الإضافية، يمكن أن تكون صفر أو أكثر
        [Range(0, double.MaxValue, ErrorMessage = "Additional charges cannot be negative.")]
        public decimal AdditionalCharges { get; set; } = decimal.Zero;

        // المبلغ الإجمالي الذي يجب دفعه بعد العودة (يشمل رسوم إضافية)
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Actual total due amount must be greater than 0.")]
        public decimal ActualTotalDueAmount { get; set; }

        // Navigation property (علاقة مع RentalTransaction)
        public RentalTransaction? RentalTransaction { get; set; } // one to one

    }
}
