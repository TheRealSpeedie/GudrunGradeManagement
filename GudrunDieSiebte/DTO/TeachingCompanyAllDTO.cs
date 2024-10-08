namespace GudrunDieSiebte.DTO
{
    public class TeachingCompanyAllDTO : TeachingConpanyDTO
    {
        public virtual ICollection<Student_ClassDTO> Students { get; set; }
    }
}
