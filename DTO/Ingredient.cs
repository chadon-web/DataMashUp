namespace DataMashUp.DTO
{
    public class Ingredient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
    }

    public class Meal
    {
        public string Type { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }

    public class DailyPlan
    {
        public List<Meal> Meals { get; set; }
    }

    public class DietPlan
    {
        public string DietType { get; set; }
        public int WeightGoal { get; set; }
        public int DietDuration { get; set; }
        public List<DailyPlan> DailyPlan { get; set; }
    }
}
