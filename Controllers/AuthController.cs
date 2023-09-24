using DataMashUp.Data;
using DataMashUp.DTO;
using DataMashUp.Migrations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace DataMashUp.Controllers
{
	[AllowAnonymous]
	public class AuthController : Controller
	{
		private readonly UserManager<IdentityUser<long>> _userManager;
		private readonly SignInManager<IdentityUser<long>> _signInManager;
		private readonly ApplicationDbContext _applicationDbContext;

		public AuthController(UserManager<IdentityUser<long>> userManager,
			SignInManager<IdentityUser<long>> signInManager,
			ApplicationDbContext applicationDbContext)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			this._applicationDbContext = applicationDbContext;
		}

		public async Task<IActionResult> Login(string returnUrl = null)
		{

			returnUrl = returnUrl ?? Url.Content("~/");

			returnUrl = "ifeanyi";
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
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return View();
				}
			}

			return View();
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
					ModelState.AddModelError("Email", "Invalid login attempt.");
					return View();

				}


				 await _signInManager.PasswordSignInAsync(register.Email,
				register.Password, false, lockoutOnFailure: false);

				return RedirectToAction("Index", "Home");

			}

			return View();
		}

	}
}
