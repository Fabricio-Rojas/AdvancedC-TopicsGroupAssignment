﻿using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AdvancedC_TopicsGroupAssignment.Models
{
    public class Business
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [MinLength(3, ErrorMessage = "Name must have at least 3 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Invalid phone number format. Use ###-###-####.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        public HashSet<Address> Addresses { get; set; }
        public HashSet<BusinessPerson> BusinessPersons { get; set; }
    }
}
