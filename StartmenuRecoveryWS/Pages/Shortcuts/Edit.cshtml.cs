using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StartmenuRecoveryWS.Data;

namespace StartmenuRecoveryWS.Pages.Shortcuts
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly StartmenuRecoveryWS.Data.ApplicationDbContext _context;

        public EditModel(StartmenuRecoveryWS.Data.ApplicationDbContext context)
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

            var shortcut =  await _context.Shortcuts.FirstOrDefaultAsync(m => m.Id == id);
            if (shortcut == null)
            {
                return NotFound();
            }
            Shortcut = shortcut;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Shortcut).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShortcutExists(Shortcut.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ShortcutExists(int id)
        {
          return (_context.Shortcuts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
