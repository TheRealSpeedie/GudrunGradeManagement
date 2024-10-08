using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public enum Types
        {
            Sehr_Wichtig,
            Wichtig,
            Normal,
            Unwichtig
        }
        public Types Type { get; set; }
        public DateTime CreateTime { get; set; }
        [ForeignKey("Teacher")]
        public int fk_Teacher { get; set; }
        public virtual Teacher Teacher { get; set; }
        [ForeignKey("Class")]
        public int fk_Class { get; set; }
        public virtual Class Class { get; set; }
        [ForeignKey("Modul")]
        public int fk_Modul { get; set; }
        public virtual Modul Modul { get; set; }
    }
}
