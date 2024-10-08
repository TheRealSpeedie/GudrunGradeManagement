namespace GudrunDieSiebte.Models
{
    public class SchoolManagement
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
