using System;
using System.Collections.Generic;

namespace LMSG3.Core.Models.Entities
{
    public class LiteratureAuthor
    {
        public int Id {  get; set; }
        public string FirstName {  get; set; }
        public string LastName {  get; set; }
        public DateTime DateOfBirth {  get; set; }

        // NAV
        public ICollection<Literature> Literatures {  get; set; }

    }
}