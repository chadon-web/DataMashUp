using System.ComponentModel.DataAnnotations;

namespace DataMashUp.Models
{
	public class DespokeSettings
	{
		[Key]
		public long Id { get; set; }
		public string APIKey { get; set; } = "";
		public string BespokeBaseUrl { get; set; } = "";

		public string UserKey { get; set; } = "";
		public bool UseTestkey { get; set; } = true;
		public string nutritionRequestUrl { get; set; } = "";

		public string Auxilary { get; set; } = "";
	}
}
