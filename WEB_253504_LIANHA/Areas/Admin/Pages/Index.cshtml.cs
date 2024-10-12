using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WEB_253504_LIANHA.Areas.Admin.Pages
{
	[Authorize(Roles = "POWER_USER")]
	public class IndexModel : PageModel
    {
        private readonly IAutomobileService _automobileService;

        public IndexModel(IAutomobileService automobileService)
        {
            _automobileService = automobileService;
        }

        public IList<Automobile> Automobile { get;set; } = default!;

		public async Task OnGetAsync()
        {
            Automobile = (await _automobileService.GetAutomobileListAsync()).Data!.Items;
        }
    }
}
