using GudrunDieSiebte.DTO;
using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.Utility.ObjectClasses
{
    public class ClassWithStudentsGrades:Class
    {
        public ICollection<Student> myStudents { get; set; }
        public int Highscore { get; set; }
    }
}
