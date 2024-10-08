using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class Student_ClassDTO : StudentDTO
    {
        public virtual ClassDTO Class { get; set; }
    }
}
