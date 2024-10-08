using GudrunDieSiebte.DTO;
using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.Utility.ObjectClasses
{
    public class StudentWithGrades:Student
    {
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
