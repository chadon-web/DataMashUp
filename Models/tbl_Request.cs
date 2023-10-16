using System.ComponentModel.DataAnnotations;

namespace DataMashUp.Models
{
	public class Request
	{
		[Key]
		public long Id { get; set; }
		public DateTime? Date { get; set; }
		public string FullName { get; set; }
		public string ActivityLevel { get; set; } = string.Empty;
		public int WeightGoal { get; set; }
		public int Duration { get; set; } = 7;
		public string Gender { get; set; } = string.Empty;
		public string WeightRecomendation { get; set; } = string.Empty;
		public int Age { get; set; }

		public string DietPreference { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;

		public string HealthCondition { get; set; } = string.Empty;

		public string BMI { get; set; } = string.Empty;

		public string Height { get; set; } = "";
		public string Weight { get; set; } = "";
		public string DietRecomended { get; set; } = "";
	


	}
}
