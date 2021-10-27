using System;
using System.ComponentModel.DataAnnotations;

namespace LMSG3.Core.Models.ViewModels
{
    public class StudentAssignmentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Assignment Name")]
        public string AssignmentName { get; set; }

        [Display(Name = "Student Name")]
        public string StudentName { get; set; }
        public DateTime UploadDate { get; set; }
        public string Path { get; set; }

    }
}