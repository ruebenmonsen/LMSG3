﻿using System;
using System.Collections.Generic;

namespace LMSG3.Core.Models.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // FK
        public int CourseId { get; set; }

        // NAV
        public Course Course { get; set; }
        public ICollection<Teacher> Teacher { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Activity> Activities { get; set; }


    }
}