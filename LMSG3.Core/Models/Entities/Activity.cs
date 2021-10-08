using System;
using System.Collections.Generic;

namespace LMSG3.Core.Models.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // FK
        public int ModuleId { get; set; }
        public int ActivityTypeId { get; set; }


        // NAV
        public Module Module { get; set; }
        public ActivityType ActivityType { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}