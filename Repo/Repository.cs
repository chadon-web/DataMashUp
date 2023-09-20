using DataMashUp.Data;
using DataMashUp.DTO;
using DataMashUp.Utilities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;

namespace DataMashUp.Repo
{
	public class Repository : IRepository
	{
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly ApplicationDbContext _context;
		public Repository(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context)
		{
			_hostingEnvironment = hostingEnvironment;
			_context = context;
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
	
						return result ?? new NewsApiResponse();

					}
				}
			}
			catch (Exception ex)
			{
				var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Static/news.json");

				var result = await AppUtility.ReadJosnAsync<NewsApiResponse>(PhoneAbsolutePath);
				if (result != null && result.Articles.Any())
				{
					result.Articles = result.Articles.Take(4).ToList();
				}
				return result;
				//using (var fs = System.IO.File.OpenText(PhoneAbsolutePath))
				//{
				//	var result = JsonConvert.DeserializeObject<NewsApiResponse>(await fs.ReadToEndAsync());

				//	if (result != null && result.Articles.Any())
				//	{
				//		result.Articles = result.Articles.Take(4).ToList();
				//	}
				//	return result;

				//}
			}
		}

		public async Task<PrescriptionDto> GetPrescription(string healthConditionId, string age)
		{
			// Start both asynchronous operations concurrently
			var consumeTask = GetConsumeOrAvoid(healthConditionId, AdminStatic.Consume);
			var avoidTask = GetConsumeOrAvoid(healthConditionId, AdminStatic.Avoid);
			var excerciseTask =  Task.Run(()=> GetExcercisePrescriptionByAge(age));
			// Wait for both tasks to complete
			await Task.WhenAll(consumeTask, avoidTask, excerciseTask);

			var consumes = consumeTask.Result;
			var avoids = avoidTask.Result;
			var excercise = excerciseTask.Result;

			consumes = AppUtility.PickRandomItems(consumes, 5);
			avoids = AppUtility.PickRandomItems(avoids, 5);

			var result = new PrescriptionDto
			{
				Consumes = consumes,
				Avoids = avoids,
				ExerciseRecommendation = excerciseTask.Result,
			};

			return result;
		}

		private ExerciseRecommendation GetExcercisePrescriptionByAge(string age)
		{
			var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Static\\excercise.json");
			var pres = AppUtility.ReadJosnAsync<ExerciseRecommendations>(PhoneAbsolutePath).Result;
			var ageExcercise = pres.AgeGroups.FirstOrDefault(x => x.Exercises.Contains(age)) ?? new ExerciseRecommendation();
			var data = AppUtility.PickRandomItems(ageExcercise.Exercises, 3);

			var result = new ExerciseRecommendation
			{
				AgeRange = ageExcercise.AgeRange,
				Exercises = data,
				Recommendation = ageExcercise.Recommendation
			};
			return result;
		}
		private async Task<List<FoodItemList>> GetConsumeOrAvoid(string healthConditionId, string consumeOrAvoid )
		{
			SecureProtocol();
			var client = new HttpClient();
			string subscriptionID = "IRcrBIYWTuQXD_YMr9eCf";
			int healthConditionID = int.Parse(healthConditionId);
			//string consumeOrAvoid = "consume";
			int limit = 50;
			// Construct the URL with query parameters
			string url = $"https://5jocnrfkfb.execute-api.us-east-1.amazonaws.com/" +
				$"PersonalRemedies/nutridigm/api/v2/topdoordonts?subscriptionID={subscriptionID}" +
				$"&healthConditionID={healthConditionID}&consumeOrAvoid={consumeOrAvoid}&limit={limit}";

			// Create the HttpRequestMessage with the URL

			List<FoodItemList> result = new List<FoodItemList>();
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, url);
				var response = await client.SendAsync(request);
				string jsonObject = await response.Content.ReadAsStringAsync();
				 //jsonObject = "{ Items : " + jsonObject +
					//"" +
					//"}";
				var todo  = JsonConvert.DeserializeObject<List<FoodItemList>>(jsonObject);

				var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, $"Static/{consumeOrAvoid}.json");
				Task.Factory.StartNew(() => { AppUtility.WriteToJosn(healthConditionId, jsonObject); });

				return todo;
			}
			catch (Exception ex)
			{

			}
			return result;

		}


	}
}
