namespace GudrunDieSiebte.DTO
{
    public class ModulLessonDTO : ModulDTO
    {

        public virtual ICollection<LessonDTO> Lessons { get; set; }
    }
}
