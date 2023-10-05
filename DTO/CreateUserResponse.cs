namespace DataMashUp.DTO
{
    public class CreateUserResponse
    {
        public string id { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public DateTime CreateDate { get; set; }
        public string ActivityLevel { get; set; }
    }
}
