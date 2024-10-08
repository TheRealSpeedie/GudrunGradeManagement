using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class ClassAllDTO: ClassDTO
    {
        public virtual TeacherDTO Teacher { get; set; }
        public virtual ICollection<LessonWithModul> Lessons { get; set; }
        public virtual ICollection<StudentDTO> Students { get; set; }
        public virtual ICollection<NotificationDTO> Notifications { get; set; }
    }
}
