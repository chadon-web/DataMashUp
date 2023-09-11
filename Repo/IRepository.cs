using DataMashUp.DTO;

namespace DataMashUp.Repo
{
	public interface IRepository
	{
		Task<NewsApiResponse> GetBreakingNews();
		Task<NewsApiResponse> GetPrescription(string healthConditionId);
	}
}
