using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [ForeignKey("Person")]
        public int fk_Person { get; set; }
        public virtual Person Person { get; set; }
    }
}
