using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;

namespace WEB_253504_LIANHA.API.Data
{
	public class DbInitializer
	{
		public static async Task SeedData(WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();

			var pathPrefix = app.Configuration.GetValue<string>("ImageSource")!;

			await SeedCategories(dbContext);
			await SeedAutomobiles(dbContext, pathPrefix);

			await dbContext.Database.MigrateAsync();
		}

		private static async Task SeedCategories(AppDbContext dbContext)
		{
			var categories = new List<AutomobileCategory> {
				new AutomobileCategory { Name = "Coupe", NormalizedName = "coupe"},
				new AutomobileCategory { Name = "Sedan", NormalizedName = "sedan"},
				new AutomobileCategory { Name = "Hatchback", NormalizedName = "hatchback"},
				new AutomobileCategory { Name = "Targa", NormalizedName = "targa"},
				new AutomobileCategory { Name = "Roadster", NormalizedName = "roadster"},
				new AutomobileCategory { Name = "SUV", NormalizedName = "suv"},
				new AutomobileCategory { Name = "Pickup Truck", NormalizedName = "pickup"},
			};

			foreach (var c in categories)
			{
				await dbContext.AutomobileCategories.AddAsync(c);
			}

			await dbContext.SaveChangesAsync();
		}

		private static async Task SeedAutomobiles(AppDbContext dbContext, string pathPrefix)
		{
			var _categories = dbContext.AutomobileCategories;

			var automobiles = new List<Automobile> {
				new Automobile {Name="Mercedes-Benz G-Class", Description="Cubic af", CategoryId = _categories.Where(c => c.NormalizedName!.Equals("suv")).First().Id, ImageUrl = pathPrefix + "img/g_wagon.jpg", PriceInUsd = 50000 },
				new Automobile {Name="Jeep Grand Cherokee Trackhawk", Description="Crazy SUV", CategoryId = _categories.Where(c => c.NormalizedName!.Equals("suv")).First().Id, ImageUrl = pathPrefix + "img/trackhawk.jpg", PriceInUsd = 50000 },

				new Automobile {Name="Chrysler 300C", Description="Hood luxury", CategoryId = _categories.Where(c => c.NormalizedName!.Equals("sedan")).First().Id, ImageUrl = pathPrefix + "img/_300c.jpg", PriceInUsd = 50000 },
				new Automobile {Name="Audi A7", Description="Suburbs luxury", CategoryId = _categories.Where(c => c.NormalizedName!.Equals("sedan")).First().Id, ImageUrl = pathPrefix + "img/a7.jpg", PriceInUsd = 50000 },

				new Automobile {Name="Porsche Cayman", Description="Like 911, but not quite", CategoryId = _categories.Where(c => c.NormalizedName!.Equals("coupe")).First().Id, ImageUrl = pathPrefix + "img/cayman.jpg", PriceInUsd = 50000 },
				new Automobile {Name="Mercedes-Benz C-class", Description="Good ol' C-class", CategoryId = _categories.Where(c => c.NormalizedName!.Equals("coupe")).First().Id, ImageUrl = pathPrefix + "img/c_class.jpg", PriceInUsd = 50000 },

				new Automobile {Name="Ford Focus ST", Description="Looks like Fiesta", CategoryId = _categories.Where(c => c.NormalizedName !.Equals("hatchback")).First().Id, ImageUrl = pathPrefix + "img/focus_st.jpg", PriceInUsd = 50000 },
				new Automobile {Name="Ford Fiesta", Description="Looks like Focus", CategoryId = _categories.Where(c => c.NormalizedName !.Equals("hatchback")).First().Id, ImageUrl = pathPrefix + "img/fiesta.jpg", PriceInUsd = 50000 },

				new Automobile {Name="Alfa Romeo 4C Targa", Description="Red but not Ferrari", CategoryId = _categories.Where(c => c.NormalizedName !.Equals("targa")).First().Id, ImageUrl = pathPrefix + "img/targa_4c.jpg", PriceInUsd = 50000 },
				new Automobile {Name="Porsche Targa 4 GTS", Description="Avergage Porsche targa", CategoryId = _categories.Where(c => c.NormalizedName !.Equals("targa")).First().Id, ImageUrl = pathPrefix + "img/targa4gts.jpg", PriceInUsd = 50000 },

				new Automobile {Name="Porsche 918 Spyder", Description="One luxurious Porsche", CategoryId = _categories.Where(c => c.NormalizedName !.Equals("roadster")).First().Id, ImageUrl = pathPrefix + "img/_918_spyder.jpg", PriceInUsd = 50000 },
				new Automobile {Name="Tesla Roadster", Description="Dad went to space", CategoryId = _categories.Where(c => c.NormalizedName !.Equals("roadster")).First().Id, ImageUrl = pathPrefix + "img/tesla_roadster.jpg", PriceInUsd = 50000 },

				new Automobile {Name="Toyota Tundra", Description="Long boiii", CategoryId = _categories.Where(c => c.NormalizedName !.Equals("pickup")).First().Id, ImageUrl = pathPrefix + "img/tundra.jpg", PriceInUsd = 50000 },
				new Automobile {Name="Ford F350", Description="Pulls like a semi", CategoryId = _categories.Where(c => c.NormalizedName !.Equals("pickup")).First().Id, ImageUrl = pathPrefix + "img/f350.jpg", PriceInUsd = 50000 },
			};

			foreach (var c in automobiles)
			{
				await dbContext.Automobiles.AddAsync(c);
			}

			await dbContext.SaveChangesAsync();
		}
	}
}
