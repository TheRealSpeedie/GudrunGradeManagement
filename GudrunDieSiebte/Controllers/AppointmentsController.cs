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
using Microsoft.AspNetCore.Identity;
using GudrunDieSiebte.DTO;
using AutoMapper;

namespace GudrunDieSiebte.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context; 
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper; 
        private readonly BasicFunctions _basicFunctions;

        public AppointmentsController(ApplicationDbContext context, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _mapper = mapper;
            _rolemanager = roleManager;
            _usermanager = userManager;
            _signInManager = signInManager;
            _basicFunctions = new BasicFunctions(context);
        }
        private class Event
        {
            public DateTime start;
            public DateTime end;
            public string title;

            public static Event ConvertLessonToEvent(Appointment appointment)
            {
                Event myEvent = new Event();
                myEvent.start = appointment.StartTime;
                myEvent.end = appointment.EndTime;
                myEvent.title = appointment.Reason;
                return myEvent;
            }
        }
        public async Task<IActionResult> GetAppointmentFromUser()
        {
            var person = _basicFunctions.getPerson(User);
            var appointments = _context.Appointment.Where(c => c.fk_Person == person.Id).ToList();
            List<Event> finishedValues = new List<Event>();
            foreach (var appointment in appointments)
            {
                finishedValues.Add(Event.ConvertLessonToEvent(appointment));
            }
            return new JsonResult(finishedValues);
        }
        public async Task<IActionResult> Generate()
        {
            DataFactory dataFactory = new DataFactory(_context, _rolemanager, _usermanager, _signInManager);
            dataFactory.GenerateAppointments(200);
            return ApiResponses.GetResponse("");
        }



        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Appointment.Include(a => a.Person).ToListAsync();
            return ApiResponses.GetResponse(_mapper.Map<List<AppointmentPersonDTO>>(applicationDbContext));
        }
        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            var appointment = await _context.Appointment
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            return ApiResponses.GetResponse(appointment);
        }
        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Reason,fk_Person")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            ViewData["fk_Person"] = new SelectList(_context.Person, "Id", "Id", appointment.fk_Person);
            return ApiResponses.GetErrorResponse(2, "Not Valid");
        }
        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Reason,fk_Person")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            ViewData["fk_Person"] = new SelectList(_context.Person, "Id", "Id", appointment.fk_Person);
            return ApiResponses.GetErrorResponse(2, "Not Valid");

        }
        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Appointment'  is null.");
            }
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
            }
            
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }
        private bool AppointmentExists(int id)
        {
          return _context.Appointment.Any(e => e.Id == id);
        }
    }
}
