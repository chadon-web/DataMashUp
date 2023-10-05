namespace DataMashUp.DTO
{
    
    public class GetIngredientDto
    {
        public string id { get; set; }
        public string name { get; set; }
    }

   

    public class FoodCategory
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<GetIngredientDto> ingredients { get; set; }
    }


}
