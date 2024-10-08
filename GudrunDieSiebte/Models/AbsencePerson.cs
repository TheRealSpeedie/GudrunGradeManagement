using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class AbsencePerson
    {
        public int Id { get; set; }
        public bool isVisible { get; set; }
        [ForeignKey("Absence")]
        public int fk_Absence { get; set; }
        public virtual Absence Absence { get; set; }
        [ForeignKey("Person")]
        public int fk_Person { get; set; }
        public virtual Person Person { get; set; }
    }
}
