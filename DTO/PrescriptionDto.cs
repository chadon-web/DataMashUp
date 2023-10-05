namespace DataMashUp.DTO
{
	public class PrescriptionDto
	{
		public List<FoodItemList> Consumes { get; set; } = new List<FoodItemList>();
		public List<FoodItemList> Avoids { get; set; } = new List<FoodItemList>();
		public List<Diet> Diets { get; set; } = new List<Diet>();
		public int Age { get; set; }
		public string BMI { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public List <string> ExerciseRecommendation { get; set; } = new List<string>();

	}
}
