using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public int Highscore { get; set; }
        public DateTime Examday { get; set; }
        public string name { get; set; }
        public string Thema { get; set; }
        [ForeignKey("Modul")]
        public int fk_Modul { get; set; }
        public virtual Modul Modul { get; set; }
        public virtual ICollection<Grade>? grades { get; set; }

    }
}
