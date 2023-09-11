using DataMashUp.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;

namespace DataMashUp.Repo
{
	public class Repository : IRepository
	{
		private readonly IWebHostEnvironment _hostingEnvironment;

		public Repository(IWebHostEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}

		private static void SecureProtocol()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
		}

		public async Task<NewsApiResponse> GetBreakingNews()
		{
			try
			{
				SecureProtocol();
				var client = new HttpClient();
				var url = "https://newsapi.org/v2/top-headlines?country=gb&category=health&apiKey=713c275bb68d4d8d9ed13b5fc4ffe980";
				var request = new HttpRequestMessage(HttpMethod.Get, url);
				var response = await client.SendAsync(request);

				var result = new NewsApiResponse();
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					 result = JsonConvert.DeserializeObject<NewsApiResponse>(data);
					return result;
				}
				else
				{
					var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Static/news.json");
					using (var fs = System.IO.File.OpenText(PhoneAbsolutePath))
					{
						result = JsonConvert.DeserializeObject<NewsApiResponse>(await fs.ReadToEndAsync());

						if(result != null && result.Articles.Any())
						{
							result.Articles = result.Articles.Take(4).ToList();
						}
	

						return result;

					}
				}
			}
			catch (Exception ex)
			{
				var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Static/news.json");
				using (var fs = System.IO.File.OpenText(PhoneAbsolutePath))
				{
					var result = JsonConvert.DeserializeObject<NewsApiResponse>(await fs.ReadToEndAsync());

					if (result != null && result.Articles.Any())
					{
						result.Articles = result.Articles.Take(4).ToList();
					}
					return result;

				}
			}
		}

		public async Task<NewsApiResponse> GetPrescription(string healthConditionId)
		{
			SecureProtocol();
			var client = new HttpClient();
			string subscriptionID = "IRcrBIYWTuQXD_YMr9eCf";
			int healthConditionID = int.Parse(healthConditionId);
			string consumeOrAvoid = "consume";
			int limit = 50;

			// Construct the URL with query parameters
			string url = $"https://5jocnrfkfb.execute-api.us-east-1.amazonaws.com/" +
				$"PersonalRemedies/nutridigm/api/v2/topdoordonts?subscriptionID={subscriptionID}" +
				$"&healthConditionID={healthConditionID}&consumeOrAvoid={consumeOrAvoid}&limit={limit}";

			// Create the HttpRequestMessage with the URL
			var request = new HttpRequestMessage(HttpMethod.Get, url);
			var response = await client.SendAsync(request);
			var result = await response.Content.ReadAsStringAsync();

			return null;

		}


	}
}
