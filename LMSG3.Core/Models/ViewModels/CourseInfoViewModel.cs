using System;
using System.Collections.Generic;

namespace LMSG3.Core.Models.ViewModels
{
    public class CourseInfoViewModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }

        public IEnumerable<ModelListViewModel> Modelslist { get; set; }
        public IEnumerable<StudentsViewModel> Participants { get; set; }


    }
}