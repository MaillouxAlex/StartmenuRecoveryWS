using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StartmenuRecoveryWS.Data;

namespace StartmenuRecoveryWS.Pages.Shortcuts
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly StartmenuRecoveryWS.Data.ApplicationDbContext _context;

        public DeleteModel(StartmenuRecoveryWS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Shortcut Shortcut { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Shortcuts == null)
            {
                return NotFound();
            }

            var shortcut = await _context.Shortcuts.FirstOrDefaultAsync(m => m.Id == id);

            if (shortcut == null)
            {
                return NotFound();
            }
            else 
            {
                Shortcut = shortcut;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Shortcuts == null)
            {
                return NotFound();
            }
            var shortcut = await _context.Shortcuts.FindAsync(id);

            if (shortcut != null)
            {
                Shortcut = shortcut;
                _context.Shortcuts.Remove(Shortcut);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
