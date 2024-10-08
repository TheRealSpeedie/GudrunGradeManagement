using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.Utility.ObjectClasses
{
    public class ClassModulExamList
    {
        public virtual Class classe { get; set; }
        public ICollection<Modul> moduls { get; set; }
    }
}
