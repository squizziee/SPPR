using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Domain.Entities;

namespace WEB_253504_LIANHA.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IAutomobileService _automobileService;

        public DeleteModel(IAutomobileService automobileService)
        {
            _automobileService = automobileService;
        }

        [BindProperty]
        public Automobile Automobile { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var automobile = (await _automobileService.GetAutomobileByIdAsync(id ?? 0)).Data;

            if (automobile == null)
            {
                return NotFound();
            }
            else
            {
                Automobile = automobile;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var automobile = (await _automobileService.GetAutomobileByIdAsync(id ?? 0)).Data;
            if (automobile != null)
            {
                Automobile = automobile;
                await _automobileService.DeleteAutomobileAsync(id ?? 0);
            }

            return RedirectToPage("./Index");
        }
    }
}
