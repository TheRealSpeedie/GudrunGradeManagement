using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.DTO
{
    public class ClassDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ClassCourseDTO> ClassCourse { get; set; }


    }
}
