using System.Collections.Generic;

namespace LMSG3.Core.Models.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // NAV
        public ICollection<Literature> Literatures { get; set; }
    }
}