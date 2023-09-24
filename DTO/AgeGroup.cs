namespace DataMashUp.DTO
{
	public class AgeGroup
	{
		public int StartAge { get; set; }
		public int EndAge { get; set; }

		public string MemberText { get; set; } = "";
		public string AgeText { get; set; } = "";
		public List<int> Members { get; } = new List<int>();
 
	}
}
