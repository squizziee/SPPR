using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA.Domain.Entities;

namespace WEB_253504_LIANHA.API.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<Automobile> Automobiles { get; set; }
		public DbSet<AutomobileCategory> AutomobileCategories { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
