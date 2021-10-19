using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSG3.Core.Models.ViewModels
{
    public class CreateModelListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int CourseId { get; set; }
        public ICollection<ActivityListViewModel> ActivitiesList { get; set; }
    }
}