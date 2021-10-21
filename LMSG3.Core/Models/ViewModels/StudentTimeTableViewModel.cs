using System.Collections.Generic;

namespace LMSG3.Core.Models.ViewModels
{
    public class StudentTimeTableViewModel
    {
        public int Year { get; set; }
        public int Week { get; set; }
        public ICollection<StudentActivityViewModel> Activities { get; set; }
    }
}
