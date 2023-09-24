namespace DataMashUp.DTO
{
	public class ExerciseRecommendation
	{
		public string age_range { get; set; } = string.Empty;
		public List<string> exercises { get; set; } = new List<string>();
		public string recommendation { get; set; } = string.Empty;
	}

	public class ExerciseRecommendations
	{
		public List<ExerciseRecommendation> age_groups { get; set; } = new List<ExerciseRecommendation>();
	}
}
