using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class Student
    {
        public int Id { get; set; }

        public DateTime EnrollmentDate { get; set; }
        [ForeignKey("Person")]
        public int fk_Person { get; set; }
        public virtual Person Person { get; set; }
        [ForeignKey("Class")]
        public int fk_Class { get; set; }
        public virtual Class Class { get; set; }
        [ForeignKey("TeachingCompany")]
        public int fk_TeachingCompany { get; set; }
        public virtual TeachingCompany TeachingCompany { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }

    }
}
