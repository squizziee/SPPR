using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Services.CategoryService;
using WEB_253504_LIANHA.Controllers;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.FileProviders;
using WEB_253504_LIANHA.Services;
using WEB_253504_LIANHA.Services.Authentication;


namespace WEB_253504_LIANHA.Tests.Controllers
{
	public class ProductControllerTests
	{
		ServiceCollection services = new ServiceCollection();

		private readonly IAutomobileService _automobileService; // = Substitute.For<IAutomobileService>();
		private readonly IAutomobileCategoryService _categoryService; // = Substitute.For<IAutomobileCategoryService>();
		private readonly IConfiguration _configuration = Substitute.For<IConfiguration>();

		private readonly ITestOutputHelper _output;

		private ProductController CreateController()
		{
			return new ProductController(_automobileService, _categoryService);
		}

		public ProductControllerTests(ITestOutputHelper output)
		{
			services.AddHttpClient<IAutomobileService, ApiAutomobileService>(client =>
			{
				client.BaseAddress = new Uri("https://localhost:7002/api/");
			});

			services.AddHttpClient<IAutomobileCategoryService, ApiAutomobileCategoryService>(client =>
			{
				client.BaseAddress = new Uri("https://localhost:7002/api/");
			});

			services.AddScoped<IFileService, ApiFileService>();
			services.AddScoped<ITokenAccessor, KeycloakTokenAccessor>();

			services.AddHttpContextAccessor();
			var inMemorySettings = new Dictionary<string, string> {
				{"SomeSetting", "SomeValue"},
				{"AnotherSetting", "AnotherValue"}
			};

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings!)
				.Build();

			services.AddSingleton<IConfiguration>(configuration);

			_output = output;

			var serviceProvider = services.BuildServiceProvider();

			_automobileService = serviceProvider.GetRequiredService<IAutomobileService>();
			_categoryService = serviceProvider.GetRequiredService<IAutomobileCategoryService>();
		}

		[Fact]
		public async Task Index_ReturnsCorrecrCategory_WhenPassedCategory()
		{
			var controller = CreateController();
			// _categoryService.GetAutomobileCategoryListAsync().Returns(Task.FromResult(ResponseData<List<AutomobileCategory>>.Error("Не удалось загрузить категории")));

			var result = await controller.Index("sedan");

			var okResult = Assert.IsType<ViewResult>(result);
			//var message = Assert.IsType<string>(okResult.Value);

			//_output.WriteLine($"The value is: {message}");

			Assert.NotNull(okResult.ViewData["CurrentCategory"]);
			Assert.Equal("sedan", (okResult.ViewData["CurrentCategory"] as AutomobileCategory)!.NormalizedName);

			//var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
			//Assert.Equal("Не удалось загрузить категории.", notFoundResult.Value);
		}
		[Fact]
		public async Task Index_ReturnsAllCategories()
		{
			var controller = CreateController();

			var result = await controller.Index(null);

			var okResult = Assert.IsType<ViewResult>(result);

			var categoriesInViewData = okResult.ViewData["Categories"] as List<AutomobileCategory>;
			Assert.NotNull(categoriesInViewData);
			Assert.Equal(7, categoriesInViewData.Count);
		}

		[Fact]
		public async Task Index_ReturnsCorrectModel()
		{
			var controller = CreateController();

			var result = await controller.Index(null);

			var okResult = Assert.IsType<ViewResult>(result);

			var model = okResult.Model as ListModel<Automobile>;
			Assert.NotNull(model);
			Assert.Equal(0, model.CurrentPage);
			Assert.Equal(5, model.TotalPages);
			Assert.Equal(3, model.Items.Count);

		}

		[Fact]
		public async Task Index_ReturnsNotFound_WhenCategoryDoesNotExist()
		{
			var controller = CreateController();

			var result = await controller.Index("nonexistantcategory");

			var okResult = Assert.IsType<NotFoundObjectResult>(result);
		}
	}
}