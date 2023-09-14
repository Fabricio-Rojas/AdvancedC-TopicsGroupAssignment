using System.ComponentModel.DataAnnotations;

namespace AdvancedC_TopicsGroupAssignment.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string StreetName { get; set; }

        [Required]
        public int StreetNumber { get; set; }

        public int? UnitNumber { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z]\s\d[A-Za-z]\d$")]
        [StringLength(7)]
        public string PostalCode { get; set; }
    }
}
