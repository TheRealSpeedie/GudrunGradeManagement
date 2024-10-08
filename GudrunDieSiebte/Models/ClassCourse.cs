using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class ClassCourse
    {
        public int Id { get; set; }
        [ForeignKey("Class")]
        public int fk_Class { get; set; }
        public Class Class { get; set; }
        [ForeignKey("Course")]
        public int fk_Course { get; set; }
        public Course Course { get; set; }
    }
}
