using System;
using System.Collections.Generic;

namespace LMSG3.Core.Models.Dtos
{
    public class LiteratureAuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        // NAV
        public ICollection<LiteratureDto> Literatures { get; set; }
    }
}