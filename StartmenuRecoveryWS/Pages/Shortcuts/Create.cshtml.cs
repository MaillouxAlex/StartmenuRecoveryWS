using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StartmenuRecoveryWS.Data;

namespace StartmenuRecoveryWS.Pages.Shortcuts
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly StartmenuRecoveryWS.Data.ApplicationDbContext _context;

        public CreateModel(StartmenuRecoveryWS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Shortcut Shortcut { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Shortcuts == null || Shortcut == null)
            {
                return Page();
            }

            _context.Shortcuts.Add(Shortcut);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
