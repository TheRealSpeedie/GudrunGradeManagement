using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class TeacherAppointmentsPersonDTOcs
    {
        public int Id { get; set; }
        public DateTime HireDate { get; set; }
        public PersonAppointmentDTO Person { get; set; }
    }
}
