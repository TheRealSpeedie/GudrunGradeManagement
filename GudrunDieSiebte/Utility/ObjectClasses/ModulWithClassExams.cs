using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.Utility.ObjectClasses
{
    public class ModulWithClassExams
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public string Shedule { get; set; }
        public virtual ICollection<examWithAverageFromClass> exams { get; set; }
    }
}
