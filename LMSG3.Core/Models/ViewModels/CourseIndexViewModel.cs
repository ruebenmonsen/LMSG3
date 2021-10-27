using LMSG3.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.ViewModels
{
    public class CourseIndexViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Course Name")]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public int Participants { get; set; }
        public ICollection<Student> Students { get; set; }
        public IEnumerable<ModuleListViewModel> Modelslist { get; set; }

    }
}
