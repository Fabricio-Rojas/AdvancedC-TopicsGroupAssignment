using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedC_TopicsGroupAssignment.Models
{
    public class PersonAddress
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Person Id is required.")]
        [Display(Name = "Person Id")]
        public int PersonId { get; set; }

        [ForeignKey(nameof(PersonId))]
        public Person Person { get; set; }

        [Required(ErrorMessage = "Address Id is required.")]
        [Display(Name = "Address Id")]
        public int AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }
    }
}
