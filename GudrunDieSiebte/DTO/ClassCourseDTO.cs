using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class ClassCourseDTO
    {
        public int Id { get; set; }
        public CourseDTO Course { get; set; }
    }
}
