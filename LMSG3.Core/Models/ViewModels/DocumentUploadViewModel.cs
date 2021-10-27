using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.ViewModels
{
    public class DocumentUploadViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EntityName { get; set; }

        [Required]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public int DocumentTypeId { get; set; }

        [Required]
        public IFormFile SubmittedFile { get; set; }
    }
}