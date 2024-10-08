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
    public class SchoolManagementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchoolManagementsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: SchoolManagements
        public async Task<IActionResult> Index()
        {
              return ApiResponses.GetResponse(await _context.SchoolManagement.ToListAsync());
        }
        // GET: SchoolManagements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SchoolManagement == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            var schoolManagement = await _context.SchoolManagement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schoolManagement == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            return ApiResponses.GetResponse(schoolManagement);
        }
        // POST: SchoolManagements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SchoolName")] SchoolManagement schoolManagement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schoolManagement);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            return ApiResponses.GetErrorResponse(2, "Not Valid");

        }
        // POST: SchoolManagements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SchoolName")] SchoolManagement schoolManagement)
        {
            if (id != schoolManagement.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");

            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schoolManagement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolManagementExists(schoolManagement.Id))
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
            return ApiResponses.GetErrorResponse(2, "Not Valid");

        }
        // POST: SchoolManagements/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SchoolManagement == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SchoolManagement'  is null.");
            }
            var schoolManagement = await _context.SchoolManagement.FindAsync(id);
            if (schoolManagement != null)
            {
                _context.SchoolManagement.Remove(schoolManagement);
            }
            
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }

        private bool SchoolManagementExists(int id)
        {
          return _context.SchoolManagement.Any(e => e.Id == id);
        }
    }
}
