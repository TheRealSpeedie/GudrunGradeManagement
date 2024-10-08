using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public virtual PersonDTO Person { get; set; }
    }
}
