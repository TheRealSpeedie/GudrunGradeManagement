using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public float grade { get; set; }
        public bool isConfirmed { get; set; }
        [ForeignKey("Student")]
        public int fk_Student { get; set; }
        public virtual Student Student { get; set; }
        [ForeignKey("Exam")]
        public int fk_Exam { get; set; }
        public virtual Exam Exam{ get; set;}
    }
}
