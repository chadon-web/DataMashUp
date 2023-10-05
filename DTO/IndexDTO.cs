using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DataMashUp.DTO
{
	public class IndexDTO
	{
		public string FullName { get; set; }
		public int WeightGoal { get; set; }

		public int Duration { get; set; }
		public string Gender { get; set; } = string.Empty;
		public string ActivityLevel { get; set; } = string.Empty;
		public string WeightRecomendation { get; set; } = string.Empty;
		public int Age { get; set; }
		public DateTime DOB { get; set; } = DateTime.Now;

		public List<SelectListItem> Locations { get; set; } = new List<SelectListItem>();

		public string DietPreference { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;

		public string HealthCondition { get; set; } = string.Empty;

		public string BMI { get; set; } = string.Empty;
	
		public string Height { get; set; } = "";
		public string Weight { get; set; } = "";

		public string MBICondition { get; set; } = string.Empty;
		public List<GetIngredientDto> Ingredients { get; set; }

		public List<Article> Articles { get; set; } = new List<Article>();

	}
}
