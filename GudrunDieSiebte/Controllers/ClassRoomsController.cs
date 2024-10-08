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
using System.Security.Claims;

namespace GudrunDieSiebte.Controllers
{
    public class ClassRoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassRoomsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: ClassRooms
        public async Task<IActionResult> Index()
        {
              return ApiResponses.GetResponse(await _context.ClassRoom.ToListAsync());
        }
        // GET: ClassRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClassRoom == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            var classRoom = await _context.ClassRoom
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classRoom == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            return ApiResponses.GetResponse(classRoom);
        }
        // POST: ClassRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomNumber,Floor")] ClassRoom classRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classRoom);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            return ApiResponses.GetErrorResponse(2, "Not Valid");
        }
        // POST: ClassRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomNumber,Floor")] ClassRoom classRoom)
        {
            if (id != classRoom.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassRoomExists(classRoom.Id))
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
            return ApiResponses.GetErrorResponse(2, "Not Valid"); ;
        }
        // POST: ClassRooms/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClassRoom == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ClassRoom'  is null.");
            }
            var classRoom = await _context.ClassRoom.FindAsync(id);
            if (classRoom != null)
            {
                _context.ClassRoom.Remove(classRoom);
            }
            
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }
        private bool ClassRoomExists(int id)
        {
          return _context.ClassRoom.Any(e => e.Id == id);
        }
    }
}
