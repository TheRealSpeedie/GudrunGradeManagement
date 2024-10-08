using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GudrunDieSiebte.Models.Modul> Modul { get; set; }
        public DbSet<GudrunDieSiebte.Models.Lesson> Lesson { get; set; }
        public DbSet<GudrunDieSiebte.Models.Student> Student { get; set; }
        public DbSet<GudrunDieSiebte.Models.Class> Class { get; set; }
        public DbSet<GudrunDieSiebte.Models.ClassRoom> ClassRoom { get; set; }
        public DbSet<GudrunDieSiebte.Models.Teacher> Teacher { get; set; }
        public DbSet<GudrunDieSiebte.Models.Person> Person { get; set; }
        public DbSet<GudrunDieSiebte.Models.SchoolManagement> SchoolManagement { get; set; }
        public DbSet<GudrunDieSiebte.Models.TeachingCompany> TeachingCompany { get; set; }
        public DbSet<GudrunDieSiebte.Models.Exam> Exam { get; set; }
        public DbSet<GudrunDieSiebte.Models.Notification> Notification { get; set; }
        public DbSet<GudrunDieSiebte.Models.Absence> Absence { get; set; }
        public DbSet<GudrunDieSiebte.Models.Appointment> Appointment { get; set; }
        public DbSet<GudrunDieSiebte.Models.Setting> Setting { get; set; }
        public DbSet<GudrunDieSiebte.Models.Notification> Notifications { get; set; }
        public DbSet<GudrunDieSiebte.Models.Course> Course { get; set; }
        public DbSet<GudrunDieSiebte.Models.ClassCourse> ClassCourse { get; set; }
        public DbSet<GudrunDieSiebte.Models.AbsenceLesson> AbsenceLesson { get; set; }
        public DbSet<GudrunDieSiebte.Models.AbsencePerson> AbsencePerson { get; set; }
        public DbSet<GudrunDieSiebte.Models.Grade> Grade { get; set; }
        public DbSet<GudrunDieSiebte.Models.ToDo> ToDo { get; set; }
        public DbSet<GudrunDieSiebte.Models.StudentToDo> StudentToDo { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}