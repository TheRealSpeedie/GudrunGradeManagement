using Microsoft.Extensions.Hosting;

namespace GudrunDieSiebte.Models
{
    public class ClassRoom
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int Floor { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
       public override string ToString()
       {
            return Floor +". OG im Zimmer " + RoomNumber;
       }
    }
}
