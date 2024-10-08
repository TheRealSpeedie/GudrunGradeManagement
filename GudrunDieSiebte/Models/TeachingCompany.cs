namespace GudrunDieSiebte.Models
{
    public class TeachingCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; } 
        public virtual ICollection<Student> Students { get; set; }
    }
}
