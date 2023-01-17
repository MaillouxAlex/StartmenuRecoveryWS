using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StartmenuRecoveryWS.Data;

namespace StartmenuRecoveryWS.Pages.Shortcuts
{
    [Authorize]
    public class IndexModel : PageModel
    {
        
        private readonly StartmenuRecoveryWS.Data.ApplicationDbContext _context;

        public IndexModel(StartmenuRecoveryWS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Shortcut> Shortcut { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Shortcuts != null)
            {
                Shortcut = await _context.Shortcuts.Where(s => s.LnkPath.Contains("C:\\ProgramData\\Microsoft\\Windows\\Start Menu")).ToListAsync();
                //Shortcut = await _context.Shortcuts.ToListAsync();
            }
        }
    }
}
