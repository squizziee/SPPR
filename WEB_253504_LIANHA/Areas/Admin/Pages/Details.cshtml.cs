using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA.API.Data;
using WEB_253504_LIANHA.Domain.Entities;

namespace WEB_253504_LIANHA.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly WEB_253504_LIANHA.API.Data.AppDbContext _context;

        public DetailsModel(WEB_253504_LIANHA.API.Data.AppDbContext context)
        {
            _context = context;
        }

        public Automobile Automobile { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var automobile = await _context.Automobiles.FirstOrDefaultAsync(m => m.Id == id);
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
