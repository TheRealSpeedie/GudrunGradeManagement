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
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: People
        public async Task<IActionResult> Index()
        {
              return View(await _context.Person.ToListAsync());
        }
        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Person == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");

            }

            return ApiResponses.GetResponse(person);
        }
        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,Birthday,Address,City")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            return ApiResponses.GetErrorResponse(2, "Not Valid");

        }
        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,Birthday,Address,City")] Person person)
        {
            if (id != person.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Person == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Person'  is null.");
            }
            var person = await _context.Person.FindAsync(id);
            if (person != null)
            {
                _context.Person.Remove(person);
            }
            
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }
        private bool PersonExists(int id)
        {
          return _context.Person.Any(e => e.Id == id);
        }
    }
}
