using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.DTO
{
    public class ClassTeacherStudentsModulsDTO
    {
        public int Id { get; set; }
        public string classname { get; set; }
        public string teachername { get; set; }
        public virtual ICollection<ModulnameDTO> Moduls { get; set; }
        public virtual ICollection<StudentnameDTO> Students { get; set; }
    }
}
