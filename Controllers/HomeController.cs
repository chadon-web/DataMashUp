using DataMashUp.DTO;
using DataMashUp.Models;
using DataMashUp.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace DataMashUp.Controllers
{
	
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly IWebHostEnvironment _hostingEnvironment;

		private readonly string _Prescription;
		private readonly IRepository _repository;
		private readonly UserManager<IdentityUser<long>> _userManager;
		private readonly SignInManager<IdentityUser<long>> _signInManager;

		public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment, IRepository repository, UserManager<IdentityUser<long>> userManager, SignInManager<IdentityUser<long>> signInManager)
		{
			_logger = logger;
			_hostingEnvironment = hostingEnvironment;
			_repository = repository;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		//[Authorize]
		public async Task<IActionResult> Index()
		{

			var ingredients = await _repository.GetIngredient();
			var result = new List<GetIngredientDto>();

			foreach (var item in ingredients)
			{
				foreach (var item2 in item.ingredients)
				{
					var ing = new GetIngredientDto
					{
						id = item2.id,
						name = item2.name,
					};
					result.Add(ing);

				}
			}

			var news = await _repository.GetBreakingNews();
			var viewModel = new IndexDTO
			{
				Locations = await GetLOcation(),
				Articles = news.Articles.Take(5).ToList(),
				Ingredients = result
			};

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Index(IndexDTO viewModel)
		{
			if(ModelState.IsValid)
			{
				viewModel.Locations = await GetLOcation();
				return View(viewModel);
			}
			viewModel.Locations = await GetLOcation();
			var age = DateTime.Now.Year - viewModel.DOB.Year;
			var param = new { healthCondition = viewModel.HealthCondition, age = age, gender = viewModel.Gender };

			return RedirectToAction (nameof(Prescription), param);
		}

		[HttpPost]

		public async Task<IActionResult> Prescription(IndexDTO viewModel)
		{	
			var prescription = await _repository.GetPrescription(viewModel);
			
			return View(prescription);
		}

		[HttpPost]

		public async Task<IActionResult> GetAllRestaurant (IndexDTO viewModel)
		{
			var restaurants = await _repository.GetAllRestuarants(viewModel);

			return View(restaurants);
		}


	
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		[NonAction]
		public async Task<List<SelectListItem>> GetLOcation()
		{

			var PhoneAbsolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Static/location.json");
			using (var fs = System.IO.File.OpenText(PhoneAbsolutePath))
			{

				var jsonData = JsonConvert.DeserializeObject<List<LocationData>>(await fs.ReadToEndAsync());

				var locations = jsonData.Select(x => new SelectListItem
				{
					Text = x.city ,
					Value = x.city,
				}).ToList();
				//locations.Insert(0, new SelectListItem { Text = "Select location", Value = null });

				return locations;


			}


		}
	}
}