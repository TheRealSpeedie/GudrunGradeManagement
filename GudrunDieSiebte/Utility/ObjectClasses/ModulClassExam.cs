using GudrunDieSiebte.DTO;

namespace GudrunDieSiebte.Utility.ObjectClasses
{
    public class ModulClassExam
    {
        public int id { get; set; }
        public string classname { get; set; }
        public ICollection<ModulnameAverage> moduls { get; set; }

    }

}

