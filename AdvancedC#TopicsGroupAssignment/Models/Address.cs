using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedC_TopicsGroupAssignment.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Street Name is required.")]
        [StringLength(100, ErrorMessage = "Street Name cannot exceed 100 characters.")]
        [MinLength(3, ErrorMessage = "Street Name must have at least 3 characters.")]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Street Number is required.")]
        [Display(Name = "Street Number")]
        public int StreetNumber { get; set; }

        [Display(Name = "Unit Number")]
        public int? UnitNumber { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z]\s\d[A-Za-z]\d$")]
        [StringLength(7)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Business Id")]
        public int? BusinessId { get; set; }

        [ForeignKey(nameof(BusinessId))]
        public Business Business { get; set; }

        public HashSet<PersonAddress> People { get; set; }
    }
}
