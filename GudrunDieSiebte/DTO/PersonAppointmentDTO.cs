using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations;

namespace GudrunDieSiebte.DTO
{
    public class PersonAppointmentDTO:PersonDTO
    {
        public virtual ICollection<AppointmentDTO> Appointments { get; set; }
    }
}
