using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.DTO
{
    public class ClassModulExamListDTO
    {
        public virtual ClassDTO classe { get; set; }
        public ICollection<ModulExamDTO> moduls { get; set; }
    }
}
