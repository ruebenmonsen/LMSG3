using System.Collections.Generic;

namespace LMSG3.Core.Models.ViewModels
{
    public class CurrentModuleViewModel
    {
        public ICollection<AssignmentViewModel> Assignments { get; set; }
        public ICollection<StudentActivityViewModel> StudentActivities { get; set; }
    }
}