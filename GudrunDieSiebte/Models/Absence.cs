using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class Absence
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public  bool confirmed { get; set; }

        [ForeignKey("Person")]
        public int fk_Person { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<AbsenceLesson> AbsenceLesson { get; set; }
        public virtual ICollection<AbsencePerson> AbsencePerson { get; set; }

    }
}
