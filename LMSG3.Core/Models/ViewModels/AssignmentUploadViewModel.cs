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
        public int? DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDescription { get; set; }
    }
}