using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class LessonWithClassModulDTO : LessonDTO
    {
       
        public ClassDTO Class { get; set; }
      
        public ModulDTO Modul { get; set; }
    }
}
