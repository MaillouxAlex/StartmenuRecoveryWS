using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartmenuRecoveryWS.Data;
using StartmenuRecoveryWS.Class;

namespace StartmenuRecoveryWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SorcutRestoredController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SorcutRestoredController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> PostShortcut([FromBody] RestoredShorcut RestoredShorcut)
        {
                var Shorcut = _context.Shortcuts.Where(s => s.LnkPath == RestoredShorcut.LnkPath && s.LnkName == RestoredShorcut.LnkName).FirstOrDefault();

                if (null != Shorcut)
                {
                    Shorcut.Restored += 1;
                    _context.Entry(Shorcut).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ShortcutExists(Shorcut.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            return "Ok";
        }
        private bool ShortcutExists(int id)
        {
            return (_context.Shortcuts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

}
