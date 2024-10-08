using GudrunDieSiebte.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Teacher")]
        public int fk_teacher { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<ClassCourse> ClassCourse { get; set; }

    }
}
