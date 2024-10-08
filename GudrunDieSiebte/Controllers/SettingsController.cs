using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GudrunDieSiebte.Data;
using GudrunDieSiebte.Models;
using GudrunDieSiebte.Utility;

namespace GudrunDieSiebte.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Settings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Setting.Include(s => s.Person);
            return ApiResponses.GetResponse(await applicationDbContext.ToListAsync());
        }
        // GET: Settings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Setting == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            var setting = await _context.Setting
                .Include(s => s.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (setting == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            return ApiResponses.GetResponse(setting);
        }
        // POST: Settings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,fk_Person")] Setting setting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(setting);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            ViewData["fk_Person"] = new SelectList(_context.Person, "Id", "Id", setting.fk_Person);
            return ApiResponses.GetErrorResponse(2, "Not Valid");

        }
        // POST: Settings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,fk_Person")] Setting setting)
        {
            if (id != setting.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(setting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingExists(setting.Id))
                    {
                        return ApiResponses.GetErrorResponse(1, "Not Found");
                    }
                    else
                    {
                        throw;
                    }
                }
                return ApiResponses.GetResponse(nameof(Index));
            }
            ViewData["fk_Person"] = new SelectList(_context.Person, "Id", "Id", setting.fk_Person);
            return ApiResponses.GetErrorResponse(2, "Not Valid");
        }
        // POST: Settings/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Setting == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Setting'  is null.");
            }
            var setting = await _context.Setting.FindAsync(id);
            if (setting != null)
            {
                _context.Setting.Remove(setting);
            }
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }
        private bool SettingExists(int id)
        {
          return _context.Setting.Any(e => e.Id == id);
        }
    }
}
