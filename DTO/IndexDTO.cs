using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DataMashUp.DTO
{
	public class IndexDTO
	{
		public string Gender { get; set; } = string.Empty;
		public int Age { get; set; }
        public List<SelectListItem> Locations { get; set; } = new List<SelectListItem>();
		[Required]
		public string Location { get; set; } = string.Empty;

		public string HealthCondition { get; set; } = string.Empty;

		public string BMI { get; set; } = string.Empty;
		[MinLength(1)]
		public string Height { get; set; } = string.Empty;
		[MinLength(1)]
		public string Weight { get; set; } = string.Empty;

		public List<Article> Articles { get; set; } = new List<Article>();

	}
}
