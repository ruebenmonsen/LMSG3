using System;
using System.Collections.Generic;

namespace LMSG3.Core.Models.Entities
{
    public class Course 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }

        // NAV
        public ICollection<Student> Students { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<Document> Documents { get; set; }



    }
}