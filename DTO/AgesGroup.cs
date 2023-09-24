namespace DataMashUp.DTO
{
	public class AgesGroup
	{
		public int StartAge { get; set; }
		public int EndAge { get; set; }

		public int AgeText { get; set; }
		public List<int> Members { get; } = new List<int>();
	}
}
