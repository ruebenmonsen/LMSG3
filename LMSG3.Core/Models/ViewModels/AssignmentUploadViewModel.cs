using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public IFormFile SubmittedFile { get; set; }
    }
}