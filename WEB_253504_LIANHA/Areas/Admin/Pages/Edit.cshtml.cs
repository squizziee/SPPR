using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Services.AutomobileService;

namespace WEB_253504_LIANHA.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IAutomobileService _automobileService;

        public EditModel(IAutomobileService automobileService)
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

            await _automobileService.UpdateAutomobileAsync(Automobile.Id, Automobile, null);

            return RedirectToPage("./Index");
        }
    }
}
