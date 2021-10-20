using System;
using System.ComponentModel.DataAnnotations;

namespace LMSG3.Core.Models.ViewModels
{
    public class ActivityListViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Activity Name")]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }
    }
}