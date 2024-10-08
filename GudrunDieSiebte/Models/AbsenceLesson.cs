using GudrunDieSiebte.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class AbsenceLesson
    {
        public int Id { get; set; }
        [ForeignKey("Absence")]
        public int fk_Absence { get; set; }
        public virtual Absence Absence { get; set; }
        [ForeignKey("Lesson")]
        public int fk_Lesson { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}
