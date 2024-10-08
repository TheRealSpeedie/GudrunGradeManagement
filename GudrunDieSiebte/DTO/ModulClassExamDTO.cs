namespace GudrunDieSiebte.DTO
{
    public class ModulClassExamDTO
    {
        public int id { get; set; }
        public string classname { get; set; }
        public ICollection<ModulnameAverageDTO> moduls { get; set; }

    }

    
}
