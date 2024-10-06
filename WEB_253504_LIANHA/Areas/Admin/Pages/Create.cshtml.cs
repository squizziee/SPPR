using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Services;
using WEB_253504_LIANHA.Services.CategoryService;

namespace WEB_253504_LIANHA.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IAutomobileService _automobileService;
        private readonly IAutomobileCategoryService _automobileCategoryService;
        private readonly IFileService _fileService;

        public CreateModel(IAutomobileService automobileService, 
            IFileService fileService, 
            IAutomobileCategoryService automobileCategoryService)
        {
            _automobileService = automobileService;
            _automobileCategoryService = automobileCategoryService;
            _fileService = fileService;
        }

        public async Task<IActionResult> OnGet()
        {
            Categories = (await _automobileCategoryService.GetAutomobileCategoryListAsync()).Data!;
            return Page();
        }

        [BindProperty]
        public List<AutomobileCategory> Categories { get; set; } = default!;
        [BindProperty]
        public int ChosenCategoryId { get; set; } = default!;
        [BindProperty]
        public Automobile Automobile { get; set; } = default!;
        [BindProperty]
        public IFormFile UploadedImage { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Automobile.CategoryId = ChosenCategoryId;

            await _automobileService.CreateAutomobileAsync(Automobile, UploadedImage);

            return RedirectToPage("./Index");
        }
    }
}
