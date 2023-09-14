using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AdvancedC_TopicsGroupAssignment.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, ErrorMessage = "First Name cannot exceed 100 characters.")]
        [MinLength(3, ErrorMessage = "First Name must have at least 3 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters.")]
        [MinLength(3, ErrorMessage = "Last Name must have at least 3 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Invalid phone number format. Use ###-###-####.")]
        public string PhoneNumber { get; set; }

        public HashSet<Address> Addresses { get; set; }
        public HashSet<BusinessPerson> BusinessPersons { get; set; }

    }
}
