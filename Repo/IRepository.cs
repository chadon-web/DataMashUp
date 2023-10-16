using DataMashUp.DTO;

namespace DataMashUp.Repo
{
	public interface IRepository
	{
		Task<ICollection<Restaurant>> GetAllRestuarants(IndexDTO model);
		Task<NewsApiResponse> GetBreakingNews();
		Task<List<Diet>> GetDietPlan();
		Task<List<FoodCategory>> GetIngredient();
		Task<List<Diet>> GetNuritionPlanFronBespok(IndexDTO dTO);
		Task<PrescriptionDto> GetPrescription(IndexDTO model);

		Task SetDietPreference(string preferences, string userId);
	}
}
