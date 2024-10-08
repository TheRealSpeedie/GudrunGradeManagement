using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.Utility.ObjectClasses
{
    public class ClassTeacherStudentsModuls
    {
        public int Id { get; set; }
        public string classname { get; set; }
        public string teachername { get; set; }
        public virtual ICollection<Modulname> Moduls { get; set; }
        public virtual ICollection<Studentname> Students { get; set; }
    }
}
