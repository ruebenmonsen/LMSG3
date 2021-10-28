using System;
using System.ComponentModel.DataAnnotations;

namespace LMSG3.Core.Models.ViewModels
{
    public class StudentDocmentsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMMM dd}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }
        public string UserFullName { get; set; }
        public bool Own { get; set; }
        public string Path { get; set; }
    }
}