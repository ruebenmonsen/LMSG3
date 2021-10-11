using System.Collections.Generic;

namespace LMSG3.Core.Models.Dtos
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // NAV
        public ICollection<LiteratureDto> Literatures { get; set; }
    }
}