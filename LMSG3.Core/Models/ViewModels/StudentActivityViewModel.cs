using System;
using System.ComponentModel.DataAnnotations;

namespace LMSG3.Core.Models.ViewModels
{
    public class StudentActivityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ActivityTypeId { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MMMM dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MMMM dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public bool HasDocument { get; set; }
        public bool IsCurrent { get; set; }
        public bool  InCurrentModule { get; set; }
    }
}
