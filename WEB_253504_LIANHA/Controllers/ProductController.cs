using Microsoft.AspNetCore.Mvc;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Services.CategoryService;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Extensions;

namespace WEB_253504_LIANHA.Controllers
{
    public class ProductController : Controller
    {
        private IAutomobileService _service;
        private IAutomobileCategoryService _categoryService;

        public ProductController(IAutomobileService automobileService, IAutomobileCategoryService automobileCategoryService)
        {
            _service = automobileService;
            _categoryService = automobileCategoryService;
        }

        public async Task<IActionResult> Index(string? category, int pageno = 0)
        {
            var productResponse =
                await _service.GetAutomobileListAsync(category, pageno);
            if (!productResponse.Successful)
                return NotFound(productResponse.ErrorMessage);

            var categories = (await _categoryService.GetAutomobileCategoryListAsync()).Data;

            ViewBag.CurrentCategory = categories!.Where(c => c.NormalizedName == category).FirstOrDefault();
            ViewBag.Categories = categories;

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/Components/Product/_ProductListPartial.cshtml", productResponse.Data);
            }

            return View(productResponse.Data!);
        }
    }
}
