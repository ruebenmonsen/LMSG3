using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LMSG3.Core.Models.Dtos;

namespace LMSG3.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
       
        //public DbSet<Literature> Literatures { get; set; }
       
        public DbSet<Student> Students {  get; set; }
        public DbSet<Module> Modules {  get; set; }
        public DbSet<Activity> Activities {  get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ActivityType> ActivityTypes {  get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().ToTable("Student");

            modelBuilder.Entity<Teacher>().ToTable("Teacher");
        }
    }
}
