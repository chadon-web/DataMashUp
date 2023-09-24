using DataMashUp.Data;
using DataMashUp.Middlewares;
using DataMashUp.Repo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddIdentity<IdentityUser<long>, IdentityRole<long>>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 6; // Modify password requirements as needed
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddSignInManager<SignInManager<IdentityUser<long>>>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

//builder.Services.AddAuthentication("Cookies")
//		.AddCookie("Cookies", options =>
//		{
//			options.LoginPath = "/Auth/Login"; // Set the login page route
//		});

//builder.Services.AddAuthorization(options =>
//{
//	options.AddPolicy("LoggedIn", policy =>
//	{
//		policy.RequireAuthenticatedUser();
//	});
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
//app.UseMiddleware<AuthorizeMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
