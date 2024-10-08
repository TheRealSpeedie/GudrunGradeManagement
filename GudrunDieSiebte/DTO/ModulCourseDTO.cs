using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.DTO
{
    public class ModulCourseDTO : ModulDTO
    {

        public virtual ICollection<CourseDTO> Course { get; set; }
    }
}
