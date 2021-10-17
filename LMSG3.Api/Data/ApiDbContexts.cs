using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace LMSG3.Api
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<Literature> Literatures { get; set; }
        public DbSet<LiteratureAuthor> LiteratureAuthors { get; set; }
        public DbSet<LiteratureLevel> literatureLevels { get; set; }
        public DbSet<LiteratureType> literatureTypes { get; set; }
        public DbSet<Subject> LiteratureSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }

    }
}
