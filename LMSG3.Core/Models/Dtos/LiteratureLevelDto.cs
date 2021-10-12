using System.Collections.Generic;

namespace LMSG3.Core.Models.Dtos
{
    public class LiteratureLevelDto
    {
        
        public string Name { get; set; }

        // NAV
        public ICollection<LiteratureDto> Literatures { get; set; }
    }
}