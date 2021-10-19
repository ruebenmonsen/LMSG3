using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        
        public int SubId {  get; set; }
        
        public int LiteraLevelId {  get; set; }
       
        public int LiteraTypeId {  get; set; }


        // NAV
        public ICollection<LiteratureAuthor> Authors { get; set; }
        //public Subject Subject { get; set; }
        //public LiteratureLevel LiteratureLevel { get; set; }
        //public LiteratureType LiteratureType { get; set; }



    }
}
