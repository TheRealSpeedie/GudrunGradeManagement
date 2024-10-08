namespace GudrunDieSiebte.DTO
{
    public class ModulExamDTO : ModulDTO
    {
        public virtual ICollection<ExamGradeDTO> Exam { get; set; }
    }
}
