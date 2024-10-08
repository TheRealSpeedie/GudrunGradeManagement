using GudrunDieSiebte.Models;
using GudrunDieSiebte.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
	public class AbsenceWithListLessonDTO
	{
		public int id { get; set; }
		public virtual AbsencePersonDTO Absence { get; set; }
		public virtual ICollection<LessonWithModul> Lessons { get; set; }
		public virtual ICollection<PersonDTO> Persons { get; set; }
	}
}
