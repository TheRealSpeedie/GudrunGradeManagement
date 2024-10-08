using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
	public class AbsenceLessonDTO
	{
		public int Id { get; set; }
		public virtual AbsenceAllDTO Absence { get; set; }
		public virtual LessonDTO Lesson { get; set; }
	}
}
