using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class NotificationDTO
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
     
    }
}
