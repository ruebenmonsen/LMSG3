using System;
using System.Collections.Generic;

namespace LMSG3.Core.Models.Dtos
{
    public class LiteratureAuthorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime DateOfBirth { get; set; }
        public int AuthorAge => DateTime.Now.Year - DateOfBirth.Year;

        // NAV
        public ICollection<LiteratureDto> Literatures { get; set; }
    }
}