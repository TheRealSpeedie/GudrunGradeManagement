using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class AbsenceAllDTO : AbsenceDTO
    {

        [ForeignKey("Person")]
        public int fk_Person { get; set; }
        public virtual PersonDTO Person { get; set; }


    }
}
