using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSG3.Core.Models.Dtos
{
    public class LiteratureAuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DateOfBirth { get; set; }

        //public int Age => DateTime.Now.Year - DateOfBirth.Year;
        public int Age { get; set; }
        public string LatestWork { get; set; }
        public int AmoutWorks { get; set; }

        // NAV
        public ICollection<LiteratureDto> Literatures { get; set; }
    }
}