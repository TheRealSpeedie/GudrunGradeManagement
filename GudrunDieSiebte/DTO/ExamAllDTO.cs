using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class ExamAllDTO : ExamDTO
    {   
        public virtual ModulDTO Modul { get; set; }
        public virtual ICollection<GradesStudentDTO> Grades { get; set; }
    }
}
