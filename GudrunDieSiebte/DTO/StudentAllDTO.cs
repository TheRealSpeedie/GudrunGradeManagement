using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class StudentAllDTO : Student_ClassDTO
    {
        public virtual TeachingConpanyDTO TeachingCompany { get; set; }
    }
}
