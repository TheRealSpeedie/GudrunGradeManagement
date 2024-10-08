using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        public DateTime HireDate { get; set; }
        [ForeignKey("Person")]
        public int fk_Person { get; set; }
        public virtual Person Person { get; set; }
        [ForeignKey("SchoolManagement")]
        public int fk_SchoolManagement { get; set; }
        public virtual SchoolManagement SchoolManagement { get; set; }
        public virtual ICollection<Class> Class { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
       

    }
}
