using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Services;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Services.CategoryService;

namespace WEB_253504_LIANHA.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IAutomobileService _automobileService;
        private readonly IAutomobileCategoryService _automobileCategoryService;
        private readonly IFileService _fileService;

        public EditModel(IAutomobileService automobileService, IFileService fileService, IAutomobileCategoryService automobileCategoryService)
        {
            _automobileService = automobileService;
            _automobileCategoryService = automobileCategoryService;
            _fileService = fileService;
        }

        [BindProperty]
        public List<AutomobileCategory> Categories { get; set; } = default!;
        [BindProperty]
        public int ChosenCategoryId { get; set; } = default!;
        [BindProperty]
        public Automobile Automobile { get; set; } = default!;
        [BindProperty]
        public IFormFile? UploadedFile { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Categories = (await _automobileCategoryService.GetAutomobileCategoryListAsync()).Data!;

            var automobile =  (await _automobileService.GetAutomobileByIdAsync(id ?? 0)).Data;
            if (automobile == null)
            {
                return NotFound();
            }
            Automobile = automobile;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _automobileService.UpdateAutomobileAsync(Automobile.Id, Automobile, UploadedFile);

            return RedirectToPage("./Index");
        }
    }
}
