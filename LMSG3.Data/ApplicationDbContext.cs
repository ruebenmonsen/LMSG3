using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LMSG3.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Teacher>().ToTable("Teacher");

            modelBuilder.Entity<ModuleTeacher>()
                .HasKey(e => new { e.ModuleId, e.TeacherId });

            modelBuilder.Entity<ModuleTeacher>()
                .HasOne(e => e.Teacher)
                .WithMany(e => e.TeacherModules)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ModuleTeacher>()
                .HasOne(e => e.Module)
                .WithMany(e => e.ModuleTeachers)
                .OnDelete(DeleteBehavior.NoAction);
        }


        public DbSet<LMSG3.Core.Models.Entities.Course> Course { get; set; }
        public DbSet<LMSG3.Core.Models.Entities.Literature> Literature { get; set; }
    }
}
