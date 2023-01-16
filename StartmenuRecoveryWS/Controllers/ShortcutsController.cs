using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartmenuRecoveryWS.Data;
using Microsoft.AspNetCore.Authorization;


namespace StartmenuRecoveryWS.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class ShortcutsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShortcutsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Shortcuts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shortcut>>> GetShortcuts(DateTime? LastTimestamp)
        {
            if (_context.Shortcuts == null)
            {
                return NotFound();
            }
            if(null == LastTimestamp)
            {
                LastTimestamp = DateTime.MinValue;
            }
            return await _context.Shortcuts.Where(s => s.Approved == true && s.ApprovedTimestamp > LastTimestamp).ToListAsync();
        }
   
        // POST: api/Shortcuts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> PostShortcut([FromBody]List<Shortcut> shortcuts)
        {
            if (_context.Shortcuts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Shortcuts'  is null.");
            }

            foreach (var _shortcut in shortcuts)
            {
                var ShorcutExist = _context.Shortcuts.Where(s => s.LnkPath == _shortcut.LnkPath && s.AppPath == _shortcut.AppPath && s.LnkName == _shortcut.LnkName).FirstOrDefault();

                if (null != ShorcutExist)
                {
                    ShorcutExist.Hit += 1;
                    _context.Entry(ShorcutExist).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ShortcutExists(ShorcutExist.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    _context.Shortcuts.Add(_shortcut);
                    await _context.SaveChangesAsync();
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
