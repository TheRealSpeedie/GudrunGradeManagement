using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class NotificationAllDTO: NotificationDTO
    {
       
        public virtual TeacherDTO Teacher { get; set; }
        
        public virtual ClassDTO Class { get; set; }
       
        public virtual ModulDTO Modul { get; set; }
    }
}
