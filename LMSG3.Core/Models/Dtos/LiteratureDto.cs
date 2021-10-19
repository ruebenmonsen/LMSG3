using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime ReleaseDate { get; set; }

        public int SubId { get; set; }

        public int LiteraLevelId { get; set; }

        public int LiteraTypeId { get; set; }
      
        public string SubjectName { get; set; }
       
        public string LevelName { get; set; }
     
        public string LiteraTypeName { get; set; }
        // NAV
        public ICollection<LiteratureAuthorDto> Authors { get; set; }
        //public SubjectDto Subject { get; set; }
        //public LiteratureLevelDto LiteratureLevel { get; set; }
        //public LiteratureTypeDto LiteratureType { get; set; }

    }
}
