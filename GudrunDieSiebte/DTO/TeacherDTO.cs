using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class TeacherDTO
    {
        public int Id { get; set; }

        public DateTime HireDate { get; set; }
        public PersonDTO Person { get; set; }
    }
}
