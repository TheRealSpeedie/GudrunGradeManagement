using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class StudentToDo
    {
        public int Id { get; set; }
        public bool Done { get; set; }
        [ForeignKey("Student")]
        public int fk_Student { get; set; }
        public virtual Student Student { get; set; }
        [ForeignKey("ToDo")]
        public int fk_ToDo { get; set; }
        public virtual ToDo ToDo { get; set; }
    }
}
