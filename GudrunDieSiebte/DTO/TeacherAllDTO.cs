using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.DTO
{
    public class TeacherAllDTO : TeacherSchoolManagementDTO
    {
        public virtual ICollection<LessonWithClassModulDTO> Lessons { get; set; }
        public virtual ICollection<NotificationDTO> Notifications { get; set; }
    }
}
