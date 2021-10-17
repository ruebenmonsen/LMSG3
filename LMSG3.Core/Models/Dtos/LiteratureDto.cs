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

        public string SubjectName { get; set; }
        public int LiteraLevelId { get; set; }
        public string LevelName { get; set; }

        public string TypeName { get; set; }
        // NAV
        public ICollection<LiteratureAuthorDto> Authors { get; set; }
        //public SubjectDto Subject { get; set; }
        //public LiteratureLevelDto LiteratureLevel { get; set; }
        //public LiteratureTypeDto LiteratureType { get; set; }

    }
}
