using DataMashUp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataMashUp.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser<long>, IdentityRole<long>, long>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<UseProfile> tbl_UserProfile { get; set; }

		public DbSet<PrescriptionRequest> tbl_Prescription { get; set; }
		public DbSet<DespokeSettings> tbl_DespokeSettings { get; set; }

		public DbSet<Request> tbl_Request { get; set; }
	}


}
