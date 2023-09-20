namespace DataMashUp.DTO
{
	
	public class FoodItemList
	{
		public string Description { get; set; }
		public int FoodItemID { get; set; }
		public string DisplayAs { get; set; }
		public string FineFoodGroup { get; set; }
		public string CoarseFoodGroup { get; set; }
		//public string Value { get; set; }
		public string Notes { get; set; }
	}
	
	public class ToDoOrNotDto
	{
		public List<FoodItemList> Items { get; set; }
	}

}
