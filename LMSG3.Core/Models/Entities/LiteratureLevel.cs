﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSG3.Core.Models.Entities
{
    public class LiteratureLevel
    {
        
        public int Id {  get; set; }
        
        public string Name {  get; set; }

        // NAV
      //  public ICollection<Literature> Literatures { get; set; }
    }
}