using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253504_LIANHA.API.Data;
using WEB_253504_LIANHA.API.Services;
using WEB_253504_LIANHA.Domain.Entities;
using Xunit.Abstractions;

namespace WEB_253504_LIANHA.Tests
{
	public class UnitTest2
	{
		private readonly ITestOutputHelper _output;
		public UnitTest2(ITestOutputHelper output)
		{
			_output = output;
		}

		private AppDbContext CreateInMemoryDbContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			var context = new AppDbContext(options);
			return context;
		}

		[Fact]
		public async Task GetProductListAsync_ReturnsFirstPageWithThreeItemsAndCalculatesTotalPagesCorrectly()
		{
			var context = CreateInMemoryDbContext();

			var categories = new List<AutomobileCategory>
			{
				new AutomobileCategory { Name = "Category1", NormalizedName = "category-n1" },
				new AutomobileCategory { Name = "Category2", NormalizedName = "category-n2" },
				new AutomobileCategory { Name = "Category3", NormalizedName = "category-n3" }
			};

			await context.AutomobileCategories.AddRangeAsync(categories);
			await context.SaveChangesAsync();

			context.Automobiles.AddRange(
				new Automobile { Name = "Automobile 1", Description = "Description 1", PriceInUsd = 20000, CategoryId = 1 },
				new Automobile { Name = "Automobile 2", Description = "Description 2", PriceInUsd = 3410, CategoryId = 2 },
				new Automobile { Name = "Automobile 3", Description = "Description 3", PriceInUsd = 225340, CategoryId = 3 },
				new Automobile { Name = "Automobile 4", Description = "Description 4", PriceInUsd = 31250, CategoryId = 4 },
				new Automobile { Name = "Automobile 5", Description = "Description 5", PriceInUsd = 86240, CategoryId = 1 },
				new Automobile { Name = "Automobile 6", Description = "Description 6", PriceInUsd = 256660, CategoryId = 2 }
			);

			await context.SaveChangesAsync();

			var service = new AutomobileService(context);

			var result = await service.GetAutomobileListAsync(null);

			Assert.True(result.Successful);
			Assert.NotNull(result.Data);
			Assert.Equal(3, result.Data.Items.Count);

			int totalPages = (int)Math.Ceiling(6 / (double)3);
			Assert.Equal(totalPages, result.Data.TotalPages);
		}

		 [Fact]
		public async Task GetProductListAsync_ReturnsCorrectPage_WhenSpecificPageIsRequested()
		{
			var context = CreateInMemoryDbContext();

			var categories = new List<AutomobileCategory>
			{
				new AutomobileCategory { Name = "Category 1", NormalizedName = "category-n1" }
			};

			await context.AutomobileCategories.AddRangeAsync(categories);
			await context.SaveChangesAsync();

			context.Automobiles.AddRange(
				new Automobile { Name = "Automobile 1", Description = "Description 1", PriceInUsd = 20000, CategoryId = 1 },
				new Automobile { Name = "Automobile 2", Description = "Description 2", PriceInUsd = 3410, CategoryId = 1 },
				new Automobile { Name = "Automobile 3", Description = "Description 3", PriceInUsd = 225340, CategoryId = 1 },
				new Automobile { Name = "Automobile 4", Description = "Description 4", PriceInUsd = 31250, CategoryId = 1 },
				new Automobile { Name = "Automobile 5", Description = "Description 5", PriceInUsd = 86240, CategoryId = 1 },
				new Automobile { Name = "Automobile 6", Description = "Description 6", PriceInUsd = 256660, CategoryId = 1 }
			);

			await context.SaveChangesAsync();

			var service = new AutomobileService(context);

			int requestedPageNo = 1;
			int pageSize = 3;

			var result = await service.GetAutomobileListAsync(null, requestedPageNo, pageSize);

			_output.WriteLine($"{result.Data}");

			Assert.True(result.Successful);
			Assert.NotNull(result.Data);
			Assert.Equal(3, result.Data.Items.Count);
			Assert.Equal(requestedPageNo, result.Data.CurrentPage);

			Assert.Contains(result.Data.Items, m => m.Name == "Automobile 4");
			Assert.Contains(result.Data.Items, m => m.Name == "Automobile 5");
			Assert.Contains(result.Data.Items, m => m.Name == "Automobile 6");
		}

		[Fact]
		public async Task GetProductListAsync_FiltersAutomobilesByCategoryCorrectly()
		{
			var context = CreateInMemoryDbContext();

			var categories = new List<AutomobileCategory>
			{
				new AutomobileCategory { Name = "Category 1", NormalizedName = "category1" },
				new AutomobileCategory { Name = "Category 2", NormalizedName = "category2" }
			};

			await context.AutomobileCategories.AddRangeAsync(categories);
			await context.SaveChangesAsync();


			context.Automobiles.AddRange(
				new Automobile { Name = "Automobile 1", Description = "Description 1", PriceInUsd = 20000, CategoryId = 1 },
				new Automobile { Name = "Automobile 2", Description = "Description 2", PriceInUsd = 3410, CategoryId = 2 },
				new Automobile { Name = "Automobile 3", Description = "Description 3", PriceInUsd = 225340, CategoryId = 2 },
				new Automobile { Name = "Automobile 4", Description = "Description 4", PriceInUsd = 31250, CategoryId = 1 },
				new Automobile { Name = "Automobile 5", Description = "Description 5", PriceInUsd = 231250, CategoryId = 2 }
			);

			await context.SaveChangesAsync();

            var service = new AutomobileService(context);

			string categoryNormalizedName = "category2".ToLower();

			var result = await service.GetAutomobileListAsync(categoryNormalizedName);

			Assert.True(result.Successful);
			Assert.NotNull(result.Data);
			Assert.Equal(3, result.Data.Items.Count);

			Assert.Equal(0, result.Data.CurrentPage);
			Assert.Equal(1, result.Data.TotalPages);


			foreach (var automobile in result.Data.Items)
			{
				Assert.Equal(2, automobile.CategoryId);
			}
		}

		[Fact]
		public async Task GetProductListAsync_DoesNotAllowPageSizeGreaterThanMaxPageSize()
		{
			var context = CreateInMemoryDbContext();

			var categories = new List<AutomobileCategory>
		{
			new AutomobileCategory { Name = "Category1", NormalizedName = "category1" }
		};

			await context.AutomobileCategories.AddRangeAsync(categories);
			await context.SaveChangesAsync();

			for (int i = 1; i <= 25; i++)
			{
				context.Automobiles.Add(new Automobile { Name = $"Automobile {i}", Description = $"Description {i}", PriceInUsd = 15000 + i * 1000, CategoryId = 1 });
			}

			await context.SaveChangesAsync();

			var service = new AutomobileService(context);

			int requestedPageSize = 50;
			int maxPageSize = 20;

			var result = await service.GetAutomobileListAsync(null, 0, requestedPageSize);

			Assert.True(result.Successful);
			Assert.NotNull(result.Data);
			Assert.Equal(maxPageSize, result.Data.Items.Count);
			Assert.Equal(0, result.Data.CurrentPage);
		}


		[Fact]
		public async Task GetProductListAsync_ReturnsError_WhenPageNumberExceedsTotalPages()
		{
			var context = CreateInMemoryDbContext();

			var categories = new List<AutomobileCategory>
		{
			new AutomobileCategory { Name = "Городские мотоциклы", NormalizedName = "urban-bikes" }
		};

			await context.AutomobileCategories.AddRangeAsync(categories);
			await context.SaveChangesAsync();

			for (int i = 1; i <= 5; i++)
			{
				context.Automobiles.Add(new Automobile { Name = $"Automobile {i}", Description = $"Description {i}", PriceInUsd = 15000 + i * 1000, CategoryId = 1 });
			}

			await context.SaveChangesAsync();

			var service = new AutomobileService(context);

			int requestedPageNo = 3;
			int pageSize = 3;

			var result = await service.GetAutomobileListAsync(null, requestedPageNo, pageSize);

			Assert.False(result.Successful);
			Assert.Equal("No such page", result.ErrorMessage);
		}
	}
}
