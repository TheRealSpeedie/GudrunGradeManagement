namespace GudrunDieSiebte.Models
{
    public class Modul
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public string Shedule { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Exam> Exam { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<Course> Course { get; set; }
        

    }
}
