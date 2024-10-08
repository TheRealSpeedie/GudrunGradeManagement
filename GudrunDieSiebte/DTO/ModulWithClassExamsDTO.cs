using GudrunDieSiebte.Utility;

namespace GudrunDieSiebte.DTO
{
    public class ModulWithClassExamsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public string Shedule { get; set; }
        public virtual ICollection<examWithAverageFromClassDTO> exams { get; set; }
    }
}
