using DataMashUp.DTO;

namespace DataMashUp.Repo
{
	public interface IRepository
	{
		Task<NewsApiResponse> GetBreakingNews();
		Task<List<Diet>> GetDietPlan();
		Task<List<FoodCategory>> GetIngredient();
		Task<List<Diet>> GetNuritionPlanFronBespok(IndexDTO dTO);
		Task<PrescriptionDto> GetPrescription(IndexDTO model);
		Task SetDietPreference(string preferences, string userId);
	}
}
