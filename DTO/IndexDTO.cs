using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DataMashUp.DTO
{
	public class IndexDTO
	{
        public List<SelectListItem> Locations { get; set; }
		[Required]
		public string Location { get; set; }

		public string HealthCondition { get; set; }

		public string BMI { get; set; }
		[MinLength(1)]
		public string Height { get; set; }
		[MinLength(1)]
		public string Weight { get; set; }

		public List<Article> Articles { get; set; }

	}
}
