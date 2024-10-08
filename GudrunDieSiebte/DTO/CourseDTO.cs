using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Typ { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
