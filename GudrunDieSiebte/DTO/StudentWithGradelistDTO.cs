namespace GudrunDieSiebte.DTO
{
    public class StudentWithGradelistDTO:StudentDTO
    {
        public virtual ICollection<GradeDTO> Grades { get; set; }
    }
}
