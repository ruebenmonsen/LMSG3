using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.Dtos
{
    public class LiteratureDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }

        // NAV
        public SubjectDto Subject { get; set; }
        public LiteratureLevelDto LiteratureLevel { get; set; }
        public LiteratureTypeDto LiteratureType { get; set; }
        public ICollection<LiteratureAuthorDto> Authors { get; set; }
    }
}
