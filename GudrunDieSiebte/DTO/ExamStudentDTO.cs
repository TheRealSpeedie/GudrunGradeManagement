namespace GudrunDieSiebte.DTO
{
    public class ExamStudentDTO:ExamDTO
    {
        public virtual ICollection<StudentDTO> students { get; set; }

    }
}
