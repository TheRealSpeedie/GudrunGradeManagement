using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.DTO
{
    public class ModuleAllDTO : ModulDTO
    {

        public virtual ICollection<LessonDTO> Lessons { get; set; }
        public virtual ICollection<ExamDTO> Exam { get; set; }
        public virtual ICollection<NotificationDTO> Notification { get; set; }
        public virtual ICollection<CourseDTO> Course { get; set; }

    }
}
