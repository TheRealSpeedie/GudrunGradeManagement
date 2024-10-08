using GudrunDieSiebte.Utility.ObjectClasses;

namespace GudrunDieSiebte.DTO
{
    public class ClassWithStudentsGradesDTO
    {
        public ICollection<StudentWithGradelistDTO> myStudents { get; set; }
        public int Highscore { get; set; }
    }
}
