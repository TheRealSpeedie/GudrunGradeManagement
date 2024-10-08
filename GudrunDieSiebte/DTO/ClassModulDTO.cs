using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.DTO
{
    public class ClassModulDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ModulDTO> moduls { get; set; }
    }
}
