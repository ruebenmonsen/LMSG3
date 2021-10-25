using System;
using System.Collections.Generic;

namespace LMSG3.Core.Models.ViewModels
{
    public class StudentTimeTableViewModel
    {
        public int Year { get; set; }
        public int Week { get; set; }
        public int WeekPrevious { get; set; }
        public int WeekNext { get; set; }
        public bool HasWeekPrevious { get; set; }
        public bool HasWeekNext { get; set; }
        public string CurrentModuleName { get; set; }
        public IDictionary<DayOfWeek, List<StudentActivityViewModel>> Activities { get; set; }
    }
}
