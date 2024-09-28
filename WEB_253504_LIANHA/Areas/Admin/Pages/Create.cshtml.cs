using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Domain.Entities;

namespace WEB_253504_LIANHA.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IAutomobileService _automobileService;

        public CreateModel(IAutomobileService automobileService)
        {
            _automobileService = automobileService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Automobile Automobile { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _automobileService.CreateAutomobileAsync(Automobile);

            return RedirectToPage("./Index");
        }
    }
}
