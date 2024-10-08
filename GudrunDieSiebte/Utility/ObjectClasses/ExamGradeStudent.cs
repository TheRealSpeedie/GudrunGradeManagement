using GudrunDieSiebte.DTO;
using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.Utility.ObjectClasses
{
    public class ExamGradeStudent
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string thema { get; set; }
        public DateTime Examday { get; set; }
        public int Highscore { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
