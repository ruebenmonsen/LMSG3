using System.Collections.Generic;

namespace LMSG3.Core.Models.Entities
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // NAV
        public ICollection<Document> Documents { get; set; }

    }
}