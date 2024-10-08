using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Utility.ObjectClasses
{
    public class ClassModul
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Modul> moduls { get; set; }
    }
}
