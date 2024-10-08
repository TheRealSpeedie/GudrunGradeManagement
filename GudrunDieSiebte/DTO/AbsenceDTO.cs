namespace GudrunDieSiebte.DTO
{
	public class AbsenceDTO
	{
		public int Id { get; set; }
		public string Reason { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public bool confirmed { get; set; }
	}
}
