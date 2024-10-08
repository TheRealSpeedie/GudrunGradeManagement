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
using AutoMapper;
using GudrunDieSiebte.DTO;
using System.Security.Claims;

namespace GudrunDieSiebte.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly BasicFunctions _basicFunctions;

        public NotificationsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; 
            _basicFunctions = new BasicFunctions(context);

        }
        public async Task<IActionResult> GetByclass()
        {
            if (User.IsInRole(Roles.getRoleString(Roles.RoleType.Student)))
            {
                var student = _basicFunctions.getStudentWithClass(User);
                var cl = student.Class;
                var notifications = _context.Notification.Where(c => c.fk_Class == cl.Id).Include(n => n.Teacher).ThenInclude(n => n.Person).Include(n => n.Modul).ToList();
                return ApiResponses.GetResponse(_mapper.Map<List<NotificationAllDTO>>(notifications));
            }
            return ApiResponses.GetResponse("");
        }



        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Notification
                .Include(n => n.Modul)
                .Include(n => n.Teacher).ThenInclude(x => x.Person)
                .OrderByDescending(n => n.CreateTime).ToListAsync();
            
            return ApiResponses.GetResponse(_mapper.Map<List<NotificationAllDTO>>(applicationDbContext));
        }
        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Notification == null)
            {
                return ApiResponses.GetErrorResponse(1, "not Found");
            }

            var notification = await _context.Notification
                .Include(n => n.Class)
                .Include(n => n.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return ApiResponses.GetErrorResponse(1, "not Found");
            }

            return ApiResponses.GetResponse(notification);
        }
        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Message,Type,fk_Teacher,fk_Class")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notification);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            ViewData["fk_Class"] = new SelectList(_context.Class, "Id", "Id", notification.fk_Class);
            ViewData["fk_Teacher"] = new SelectList(_context.Teacher, "Id", "Id", notification.fk_Teacher);
            return ApiResponses.GetErrorResponse(2, "not Valid");
        }
        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Message,Type,fk_Teacher,fk_Class")] Notification notification)
        {
            if (id != notification.Id)
            {
                return ApiResponses.GetErrorResponse(1, "not Found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.Id))
                    {
                        return ApiResponses.GetErrorResponse(1, "not Found");
                    }
                    else
                    {
                        throw;
                    }
                }
                return ApiResponses.GetResponse(nameof(Index));
            }
            ViewData["fk_Class"] = new SelectList(_context.Class, "Id", "Id", notification.fk_Class);
            ViewData["fk_Teacher"] = new SelectList(_context.Teacher, "Id", "Id", notification.fk_Teacher);
            return ApiResponses.GetErrorResponse(2, "not Valid"); ;
        }
        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notification == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Notification'  is null.");
            }
            var notification = await _context.Notification.FindAsync(id);
            if (notification != null)
            {
                _context.Notification.Remove(notification);
            }
            
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }
        private bool NotificationExists(int id)
        {
          return _context.Notification.Any(e => e.Id == id);
        }
    }
}
