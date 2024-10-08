using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class Lesson
    {
        public int Id { get; set; }
       
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [ForeignKey("ClassRoom")]
        public int fk_ClassRoom { get; set; }
        public  ClassRoom ClassRoom { get; set; }
        [ForeignKey("Class")]
        public int fk_Class { get; set; }
        public  Class Class { get; set; }
        [ForeignKey("Teacher")]
        public int fk_Teacher { get; set; }
        public  Teacher Teacher { get; set; }
        [ForeignKey("Modul")]
        public int fk_Modul { get; set; }
        public  Modul Modul { get; set; }
        public virtual ICollection<AbsenceLesson> AbsenceLesson { get; set; }
    }
}
