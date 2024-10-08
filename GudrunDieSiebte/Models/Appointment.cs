using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace GudrunDieSiebte.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [ForeignKey("Person")]
        public int fk_Person { get; set; }
        public virtual Person Person { get; set; }
        public string ToShort( string reason)
        {
            string shortreason = Regex.Replace(reason.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
            return shortreason;
        }
    }
}
