using DataMashUp.Data;
using DataMashUp.Middlewares;
using DataMashUp.Repo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 2;
 // Modify password requirements as needed
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddSignInManager<SignInManager<IdentityUser<long>>>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();



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
//app.UseSerilogRequestLogging();

app.UseAuthorization();
//app.UseMiddleware<AuthorizeMiddleware>();
app.UseCustomExceptionMiddleware();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
