using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.DTO
{
    public class ExamGradeStudentDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string thema { get; set; }
        public DateTime Examday { get; set; }
        public int Highscore { get; set; }
        public virtual ICollection<StudentWithGradelistDTO> Students { get; set; }
    }
}
