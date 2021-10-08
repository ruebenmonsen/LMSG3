using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Models.Entities
{
    public class Literature
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }

        // FK
        public int SubjectId {  get; set; }
        public int LiteratureLevelId {  get; set; }
        public int LiteratureTypeId {  get; set; }

        // NAV
        public Subject Subject { get; set; }
        public LiteratureLevel LiteratureLevel { get; set; }
        public LiteratureType LiteratureType { get; set; }
        public ICollection<LiteratureAuthor> Authors { get; set; }

    }
}
