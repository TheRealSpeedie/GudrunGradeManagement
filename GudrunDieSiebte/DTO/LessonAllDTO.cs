using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class LessonAllDTO:LessonDTO
    {
      
        public ClassRoomDTO ClassRoom { get; set; }
        
        public ClassDTO Class { get; set; }
      
        public TeacherAppointmentsPersonDTOcs Teacher { get; set; }
       
        public ModulDTO Modul { get; set; }
    }
}
