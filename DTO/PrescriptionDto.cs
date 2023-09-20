namespace DataMashUp.DTO
{
	public class PrescriptionDto
	{
		public List<FoodItemList> Consumes { get; set; } = new List<FoodItemList>();
		public List<FoodItemList> Avoids { get; set; } = new List<FoodItemList>();
		public int Age { get; set; }
		public string Gender { get; set; } = string.Empty;
		public ExerciseRecommendation ExerciseRecommendation { get; set; } = new ExerciseRecommendation();

	}
}
