namespace DataMashUp.DTO
{
    public class DietPlanDto
    {
        public string dietType { get; set; } = "LOW_CARB";
        public int weightGoal { get; set; } // Assuming weight goal is a string, you can change it to the appropriate type
        public int dietDuration { get; set; }

    }

    public class Diet
    {
        public string Day { get; set; } 
        public string BreakFast { get; set; } // Assuming weight goal is a string, you can change it to the appropriate type
        public string Lunch { get; set; }

        public string Dinner { get; set; }
        public string SNACK { get; set; }

    }

}
