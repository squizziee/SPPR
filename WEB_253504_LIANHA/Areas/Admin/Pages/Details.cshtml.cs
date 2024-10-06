using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Services.AutomobileService;

namespace WEB_253504_LIANHA.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IAutomobileService _automobileService;

        public DetailsModel(IAutomobileService automobileService)
        {
            _automobileService = automobileService;
        }

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
    }
}
