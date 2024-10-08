using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class LessonWithModul : LessonDTO
    {
        public ModulDTO Modul { get; set; }
    }
}
