namespace GudrunDieSiebte.DTO
{
    public class ModulLessonExamDTO : ModulDTO
    {

        public virtual ICollection<LessonDTO> Lessons { get; set; }
        public virtual ICollection<ExamDTO> Exam { get; set; }
    }
}
