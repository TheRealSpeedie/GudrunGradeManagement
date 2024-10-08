using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class ToDoDTO
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public DateTime UpTo { get; set; }
        public virtual ModulDTO Modul { get; set; }
        public virtual TeacherDTO Teacher { get; set; }
        public virtual ClassDTO Class { get; set; }
    }
}
