using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSG3.Core.Models.Entities
{
    public class Module : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }

        // FK
        public int CourseId { get; set; }

        // NAV
        public Course Course { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Activity> Activities { get; set; }


        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("EndDate must be greater than StartDate");
            }
          
        }
    }
}