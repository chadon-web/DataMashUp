using DataMashUp.DTO;

namespace DataMashUp.Repo
{
	public interface IRepository
	{
		Task<NewsApiResponse> GetBreakingNews();
		Task<PrescriptionDto> GetPrescription(string healthConditionId, string age);
	}
}
