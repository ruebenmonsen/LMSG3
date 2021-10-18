using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.ViewModels
{
    public class AssignmentUploadViewModel
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}