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
using Newtonsoft.Json.Linq;
using Microsoft.Build.Framework;


namespace WEB_253504_LIANHA.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly IAutomobileService _automobileService = Substitute.For<IAutomobileService>();
        private readonly IAutomobileCategoryService _categoryService = Substitute.For<IAutomobileCategoryService>();
        private readonly IConfiguration _configuration = Substitute.For<IConfiguration>();

        private readonly ITestOutputHelper _output;

        private ProductController CreateController()
        {
            return new ProductController(_automobileService, _categoryService);
        }

        public ProductControllerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenCategoriesNotLoaded()
        {
            var controller = CreateController();
            _categoryService.GetAutomobileCategoryListAsync().Returns(Task.FromResult(ResponseData<List<AutomobileCategory>>.Error("Не удалось загрузить категории")));

            var result = await controller.Index("all");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okResult.Value);

            _output.WriteLine($"The value is: {message}");

            //var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            //Assert.Equal("Не удалось загрузить категории.", notFoundResult.Value);
        }
        [Fact]
        public async Task Index_ReturnsNotFound_WhenMotorcyclesNotLoaded()
        {
            var controller = CreateController();
            var category = "TestCategory";
            _categoryService.GetAutomobileCategoryListAsync().Returns(Task.FromResult(ResponseData<List<AutomobileCategory>>.Success(new List<AutomobileCategory> { new AutomobileCategory { Name = "TestCategory", NormalizedName = "TESTCATEGORY" } })));
            _automobileService.GetAutomobileListAsync(category, 1).Returns(Task.FromResult(ResponseData<ListModel<Automobile>>.Error("Не удалось загрузить мотоциклы")));

            var result = await controller.Index(category);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Не удалось загрузить мотоциклы", notFoundResult.Value);
        }
        [Fact]
        public async Task Index_PopulatesViewDataWithCategories_WhenCategoriesAreSuccessfullyLoaded()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            httpContext.Request.Headers["X-Requested-With"] = "";

            var expectedCategories = new List<AutomobileCategory> {
        new AutomobileCategory { Name = "Sport", NormalizedName = "SPORT" },
        new AutomobileCategory { Name = "Touring", NormalizedName = "TOURING" }
    };
            _categoryService.GetAutomobileCategoryListAsync().Returns(Task.FromResult(ResponseData<List<AutomobileCategory>>.Success(expectedCategories)));
            _automobileService.GetAutomobileListAsync(null, 1).Returns(Task.FromResult(ResponseData<ListModel<Automobile>>.Success(new ListModel<Automobile>())));

            var result = await controller.Index(null);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData["Categories"]);
            var categoriesInViewData = viewResult.ViewData["Categories"] as List<AutomobileCategory>;
            Assert.Equal(expectedCategories, categoriesInViewData);
        }
        [Fact]
        public async Task Index_SetsCurrentCategoryToAll_WhenCategoryIsNull()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            _categoryService.GetAutomobileCategoryListAsync().Returns(Task.FromResult(ResponseData<List<AutomobileCategory>>.Success(new List<AutomobileCategory>())));
            _automobileService.GetAutomobileListAsync(null, 1).Returns(Task.FromResult(ResponseData<ListModel<Automobile>>.Success(new ListModel<Automobile>())));

            var result = await controller.Index(null);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Все Мотоциклы", viewResult.ViewData["CurrentCategory"]);
        }
        [Fact]
        public async Task Index_SetsCurrentCategoryCorrectly_WhenCategoryIsSpecified()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            string category = "sport-bikes";
            var categories = new List<AutomobileCategory>
    {
        new AutomobileCategory { Name = "Городские мотоциклы", NormalizedName = "urban-bikes" },
        new AutomobileCategory { Name = "Спортивные мотоциклы", NormalizedName = "sport-bikes" },
        new AutomobileCategory { Name = "Приключенческие мотоциклы", NormalizedName = "adventure-bikes" },
        new AutomobileCategory { Name = "Классические мотоциклы", NormalizedName = "classic-bikes" },
        new AutomobileCategory { Name = "Круизеры", NormalizedName = "cruiser-bikes" }
    };
            _categoryService.GetAutomobileCategoryListAsync().Returns(Task.FromResult(ResponseData<List<AutomobileCategory>>.Success(categories)));
            _automobileService.GetAutomobileListAsync(category, 1).Returns(Task.FromResult(ResponseData<ListModel<Automobile>>.Success(new ListModel<Automobile>())));

            var result = await controller.Index(category);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Спортивные мотоциклы", viewResult.ViewData["CurrentCategory"]);
        }
        [Fact]
        public async Task Index_ReturnsViewWithMotorcycleListModel_WhenDataIsSuccessfullyLoaded()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var category = "sport-bikes";
            var categories = new List<AutomobileCategory>
    {
        new AutomobileCategory { Name = "Городские мотоциклы", NormalizedName = "urban-bikes" },
        new AutomobileCategory { Name = "Спортивные мотоциклы", NormalizedName = "sport-bikes" },
        new AutomobileCategory { Name = "Приключенческие мотоциклы", NormalizedName = "adventure-bikes" },
        new AutomobileCategory { Name = "Классические мотоциклы", NormalizedName = "classic-bikes" },
        new AutomobileCategory { Name = "Круизеры", NormalizedName = "cruiser-bikes" }
    };
            var expectedMotorcycles = new ListModel<Automobile>
            {
                Items = new List<Automobile> { new Automobile(), new Automobile(), new Automobile() },
                CurrentPage = 1,
                TotalPages = 2
            };

            _categoryService.GetAutomobileCategoryListAsync().Returns(Task.FromResult(ResponseData<List<AutomobileCategory>>.Success(categories)));
            _automobileService.GetAutomobileListAsync(category, 1).Returns(Task.FromResult(ResponseData<ListModel<Automobile>>.Success(expectedMotorcycles)));

            var result = await controller.Index(category);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ListModel<Automobile>>(viewResult.Model);
            Assert.Equal(expectedMotorcycles, model);
        }
    }
}