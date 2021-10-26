using System;

namespace LMSG3.Core.Models.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public DateTime UploadDate { get; set; }

        // FK
        public int DocumentTypeId { get; set; }
        public string ApplicationUserId { get; set; }
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }

        // NAV
        public DocumentType DocumentType { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Course Course { get; set; }
        public Module Module { get; set; }
        public Activity Activity { get; set; }

    }

   
}