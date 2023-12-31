﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedC_TopicsGroupAssignment.Models
{
    public class BusinessPerson
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Business Id is required.")]
        [Display(Name = "Business Id")]
        public int BusinessId { get; set; }

        [ForeignKey(nameof(BusinessId))]
        public Business Business { get; set; }

        [Required(ErrorMessage = "Business Id is required.")]
        [Display(Name = "Person Id")]
        public int PersonId { get; set; }

        [ForeignKey(nameof(PersonId))]
        public Person Person { get; set; }
    }
}
