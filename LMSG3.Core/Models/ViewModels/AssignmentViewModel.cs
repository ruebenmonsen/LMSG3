using System;
using System.ComponentModel.DataAnnotations;

namespace LMSG3.Core.Models.ViewModels
{
    public class AssignmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMMM dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsSubmitted { get; set; }

    }
}