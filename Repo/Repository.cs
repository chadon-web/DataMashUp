using DataMashUp.Data;
using DataMashUp.DTO;
using DataMashUp.Models;
using DataMashUp.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace DataMashUp.Repo
{
	public class Repository : IRepository
	{
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly ApplicationDbContext _context;
		private readonly SignInManager<IdentityUser<long>> _signInManager;
		private readonly UserManager<IdentityUser<long>> _userManager;
		private  DespokeSettings _despokeSettings  { get; set; }
		private IConfiguration configuration { get; set; }


		public Repository(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context, SignInManager<IdentityUser<long>> signInManager, UserManager<IdentityUser<long>> userManager)
		{
			_hostingEnvironment = hostingEnvironment;
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;

			 configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json") // Assuming your JSON configuration is in a file named appsettings.json
			.Build();

			_despokeSettings = _context.tbl_DespokeSettings.FirstOrDefault() ??  new DespokeSettings();

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

		public async Task<PrescriptionDto> GetPrescription(IndexDTO model)
		{
			
			var age = DateTime.Now.Year - model.DOB.Year;
			var excerciseTask =  Task.Run(()=> GetExcercisePrescriptionByAge(age.ToString()));

			if (!_despokeSettings.UseTestkey)
			{
				var userId = await GetUserId(model);
				_despokeSettings.UserKey = userId.id;
			}

			if (!string.IsNullOrEmpty(model.DietPreference))
			{
				await SetDietPreference(model.DietPreference, "");
			}

			var dietTask = Task.Run( async() => await GetNuritionPlanFronBespok(model));
			//Wait for both tasks to complete

			 await Task.WhenAll(dietTask, excerciseTask);

			var result = new PrescriptionDto
			{

				ExerciseRecommendation = excerciseTask.Result,
				Diets = dietTask.Result,
				Age = age,
				Name = model.FullName,
				Gender = model.Gender,
				BMI = model.BMI,
			};


			var request = new Request
			{
				Weight = model.Weight,
				WeightGoal = model.WeightGoal,
				Age = age,
				Location = model.Location,
				FullName = model.FullName,
				Gender = model.Gender,
				HealthCondition = model.HealthCondition,
				WeightRecomendation = model.WeightRecomendation,
				Height = model.Height,
				DietPreference = model.DietPreference ?? "",
				BMI = model.BMI,
				Duration = 7,
				DietRecomended = JsonConvert.SerializeObject(dietTask.Result),
				ActivityLevel = model.ActivityLevel,
			};

			_context.Add(request);
			await _context.SaveChangesAsync();

			return result;
		}

		private List<string> GetExcercisePrescriptionByAge(string age)
		{
			var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Static/excercise.json");
			var pres = AppUtility.ReadJosnAsync<ExerciseRecommendations>(PhoneAbsolutePath).Result;
			var ageExcercise = AppUtility.GetExcercise(pres, age);

			return ageExcercise;
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




		

		public async Task<List<FoodCategory>> GetIngredient()
		{
			string newAPIKey = configuration["Settings:newAPIKey"];
			string baseUrl = configuration["Settings:BespokeBaseUrl"];
			string requestUri = configuration["Settings:nutritionRequestUrl"];
			string testUser = configuration["Settings:testUser"];
			string enableTestUser = configuration["Settings:enableTestUser"];

			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, "https://bespoke-diet-generator.p.rapidapi.com/ingredients");
			request.Headers.Add("X-RapidAPI-Key", _despokeSettings.APIKey);
			request.Headers.Add("X-RapidAPI-Host", _despokeSettings.BespokeBaseUrl);
			request.Headers.Add("accept-language", "en");
			var response = await client.SendAsync(request);
			var obj = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<List<FoodCategory>>(obj);

			return result;

		}




		private string BuildMealString(DailyPlan item, string mealType)
		{
			var mealIngredients = item.Meals.FirstOrDefault(x => x.Type == mealType)?.Ingredients;
			var mealStringBuilder = new StringBuilder();

			foreach (var ingredient in mealIngredients ?? Enumerable.Empty<Ingredient>())
			{
				mealStringBuilder.Append($"{ingredient.Name} (quantity: {ingredient.Quantity} g), ");
			}

			return mealStringBuilder.ToString();
		}


		public async  Task SetDietPreference(string preferences, string userId)
		{
			string newAPIKey = configuration["Settings:newAPIKey"];
			string baseUrl = configuration["Settings:BespokeBaseUrl"];
			string requestUri = configuration["Settings:nutritionRequestUrl"];
			string testUser = _despokeSettings.UserKey;
			string enableTestUser = configuration["Settings:enableTestUser"];

			userId = requestUri;
			var payload = new SetDietPreferenceDto();

			var ids = preferences.Split(',').ToList();
			foreach (var item in ids)
			{
				payload.ingredientIds.Add(item);
			}

			var cont = JsonConvert.SerializeObject(payload);

			var url = requestUri + testUser + "/ingredients/excluded";
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Put, url);
			request.Headers.Add("accept-language", "en");
			request.Headers.Add("Accept", "application/json");
			request.Headers.Add("X-RapidAPI-Key", _despokeSettings.APIKey);
			request.Headers.Add("X-RapidAPI-Host", _despokeSettings.BespokeBaseUrl);
			var content = new StringContent(cont, null, "application/json");
			request.Content = content;
			var response = await client.SendAsync(request);

		}
		public async Task<List<Diet>> GetDietPlan()
		{
			var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Static/nutrient.json");
			var diets = new List<Diet>();
			using (var fs = System.IO.File.OpenText(PhoneAbsolutePath))
			{
				var result = JsonConvert.DeserializeObject<DietPlan>(await fs.ReadToEndAsync());

				var day = 0;
				foreach (var item in result.DailyPlan)
				{
					day++;
					var diet = new Diet
					{
						BreakFast = BuildMealString(item, "BREAKFAST"),
						Lunch = BuildMealString(item, "LUNCH"),
						Dinner = BuildMealString(item, "DINNER"),
						SNACK = BuildMealString(item, "SNACK1"),
						Day = "Day " + day
					};
					diets.Add(diet);
				}
				return diets;

			}
		}

		public async Task<List<Diet>> GetNuritionPlanFronBespok(IndexDTO dTO)
		{

			
			string requestUri = configuration["Settings:nutritionRequestUrl"];
			string testUser = _despokeSettings.UserKey;
			string enableTestUser = configuration["Settings:enableTestUser"] ;


			var payload = new DietPlanDto
			{
				weightGoal = dTO.WeightGoal,
				dietDuration = 7
			};
			
			var cont = JsonConvert.SerializeObject(payload);

			var url = $"{requestUri}{testUser}/diet";
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Put, url);
			request.Headers.Add("accept-language", "en");
			request.Headers.Add("Accept", "application/json");
			request.Headers.Add("X-RapidAPI-Key", _despokeSettings.APIKey);
			request.Headers.Add("X-RapidAPI-Host", _despokeSettings.BespokeBaseUrl);
			var content = new StringContent(cont, null, "application/json");
			request.Content = content;

			using (var response = await client.SendAsync(request))
			{
				response.EnsureSuccessStatusCode();
				var body = await response.Content.ReadAsStringAsync();

				var result = JsonConvert.DeserializeObject<DietPlan>(body);
				var diets = new List<Diet>();

				var day = 0;
				foreach (var item in result.DailyPlan)
				{
					day++;
					var diet = new Diet
					{
						BreakFast = BuildMealString(item, "BREAKFAST"),
						Lunch = BuildMealString(item, "LUNCH"),
						Dinner = BuildMealString(item, "DINNER"),
						SNACK = BuildMealString(item, "SNACK1"),
						Day = "Day " + day
					};
					diets.Add(diet);
				}
				return diets;


			}

		}



		public async Task<CreateUserResponse> GetUserId(IndexDTO dTO)
		{

			string requestUri = configuration["Settings:nutritionRequestUrl"];
			var payload = new UserInfo
			{
				weight = double.Parse(dTO.Weight),
				height = double.Parse(dTO.Height) * 100,
				dateOfBirth = dTO.DOB,
				sex = dTO.Gender,
				activityLevel = dTO.ActivityLevel
			};


			var cont = JsonConvert.SerializeObject(payload);
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, "https://bespoke-diet-generator.p.rapidapi.com/user");
			request.Headers.Add("accept-language", "en");
			request.Headers.Add("Accept", "application/json");
			request.Headers.Add("X-RapidAPI-Key", _despokeSettings.APIKey);
			request.Headers.Add("X-RapidAPI-Host", _despokeSettings.BespokeBaseUrl);
			var content = new StringContent(cont, null, "application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			var body = await response.Content.ReadAsStringAsync();
			response.EnsureSuccessStatusCode();
		
			var result =  JsonConvert.DeserializeObject<CreateUserResponse>(body);
			return result;

		}



	}
}
