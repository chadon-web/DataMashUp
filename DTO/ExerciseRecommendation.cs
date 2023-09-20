namespace DataMashUp.DTO
{
	public class ExerciseRecommendation
	{
		public string AgeRange { get; set; } = string.Empty;
		public List<string> Exercises { get; set; } = new List<string>();
		public string Recommendation { get; set; } = string.Empty;
	}

	public class ExerciseRecommendations
	{
		public List<ExerciseRecommendation> AgeGroups { get; set; } = new List<ExerciseRecommendation>();
	}
}
