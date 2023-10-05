using DataMashUp.Data;
using DataMashUp.DTO;
using DataMashUp.Migrations;
using DataMashUp.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Text;
using System.Transactions;

namespace DataMashUp.Controllers
{
	[AllowAnonymous]

	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser<long>> _userManager;
		private readonly SignInManager<IdentityUser<long>> _signInManager;
		private readonly ApplicationDbContext _applicationDbContext;
		private readonly IHttpContextAccessor contextAccessor;
		private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
		public string rootPath { get; set; }
		public AccountController(UserManager<IdentityUser<long>> userManager,
			SignInManager<IdentityUser<long>> signInManager,
			ApplicationDbContext applicationDbContext,
			IHttpContextAccessor contextAccessor,
			Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			this._applicationDbContext = applicationDbContext;
			this.contextAccessor = contextAccessor;
			_hostingEnvironment = hostingEnvironment;
			rootPath = _hostingEnvironment.ContentRootPath;
		}


		public async Task<IActionResult> Login(string returnUrl = null)
		{

			returnUrl = returnUrl ?? Url.Content("~/");

			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
			ViewData["returnUrl"] = returnUrl;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{

			var returnUrl = loginDto.ReturnUrl;
			returnUrl = returnUrl ?? Url.Content("~/");

			if (ModelState.IsValid)
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true
				var userLogin = _userManager.Users.FirstOrDefault(x => x.Email == loginDto.Email);
				if (userLogin == null)
				{
					ModelState.AddModelError(string.Empty, "user does not exist.");
					return View();
				}
				if (userLogin.PhoneNumberConfirmed)
				{
					ModelState.AddModelError(string.Empty, "your profile has been diabled, kindly contact the bank");
					return View();
				}
				var result = await _signInManager.PasswordSignInAsync(loginDto.Email,
					loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);
				if (result.IsLockedOut)
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return View();
				}
				if (result.Succeeded)
				{
					//_logger.LogInformation("User logged in.");
					return LocalRedirect(returnUrl);
				}
				if (result.RequiresTwoFactor)
				{
					return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = loginDto.RememberMe });
				}
				else
				{
					ModelState.AddModelError("Email", "Invalid login attempt.");
					return View();
				}
			}

			return View();
		}

		public async Task<IActionResult> ConfirmEmail(string returnUrl = null)
		{

			returnUrl = returnUrl ?? Url.Content("~/");

			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
			ViewData["returnUrl"] = returnUrl;

			return View();
		}

		public string GenerateEmailConfirmLink(IdentityUser<long> user)
		{
			var scheme = contextAccessor.HttpContext.Request.Scheme;
			var host = contextAccessor.HttpContext.Request.Host.Value;
			var token =  _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
			var fileurl = $"{scheme}://{host}/ConfirmEmail";
			var baseUrl = $"{fileurl}?token={token}&email={user.Email}";

			return baseUrl;
		}

		public async Task<IActionResult> Register()
		{

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterDto register)
		{

			if(ModelState.IsValid)
			{
				var user = new IdentityUser<long>
				{
					UserName = register.Email,
					Email = register.Email,
					EmailConfirmed = true
				};
				
				var result = await _userManager.CreateAsync(user,register.Password);
				if (!result.Succeeded)
				{
					var error = new StringBuilder();
					foreach (var item in result.Errors)
					{
						error.Append(item.Description + ", ");
					}
					ModelState.AddModelError("Email", error.ToString());
					return View();

				}
				var url = GenerateEmailConfirmLink(user);
				var emailTeplate = new EmailRequest
				{
					To = user.Email,
					Body = AppUtility.FormatConfirmEmailTemplate(url, rootPath, user.UserName)

				};
				Task.Run(async () => await AppUtility.SendMail(emailTeplate));

				 await _signInManager.PasswordSignInAsync(register.Email,
				register.Password, false, lockoutOnFailure: false);

				return RedirectToAction("Index", "Home");

			}

			return View();
		}

	}
}
