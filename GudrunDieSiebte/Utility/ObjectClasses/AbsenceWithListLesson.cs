using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.Utility
{
	public class AbsenceWithListLesson
	{
		public int id { get; set; }

		[ForeignKey("Absence")]
		public int fk_Absence { get; set; }
		public virtual Absence Absence { get; set; }
		public virtual ICollection<Lesson> Lessons { get; set; }
		public virtual ICollection<Person> Persons { get; set; }
	}
}
