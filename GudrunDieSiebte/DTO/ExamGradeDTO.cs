namespace GudrunDieSiebte.DTO
{
    public class ExamGradeDTO : ExamDTO
    {
        public virtual ICollection<GradeDTO> Grades { get; set; }
    }
}
