using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Typ { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [ForeignKey("Modul")]
        public int modulId { get; set; }
        public virtual Modul modul { get; set; }
        public virtual ICollection<ClassCourse> ClassCourse { get; set; }
    }
}
