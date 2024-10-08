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
using GudrunDieSiebte.DTO;
using AutoMapper;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Aspose.Pdf.Drawing;
using Microsoft.CodeAnalysis.Scripting;
using GudrunDieSiebte.Utility.ObjectClasses;

namespace GudrunDieSiebte.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly SignInManager<IdentityUser> _signInManager; 
        private readonly BasicFunctions _basicFunctions;

        public LessonsController(ApplicationDbContext context, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager )
        {
            _context = context;
            _mapper = mapper;
            _rolemanager = roleManager;
            _usermanager = userManager;
            _signInManager = signInManager; 
            _basicFunctions = new BasicFunctions(context);
        }
        public async Task<IActionResult> GetLessonFromUser()
        {
            if (User.IsInRole(Roles.getRoleString(Roles.RoleType.Student)))
            {
                var student = _basicFunctions.getStudentWithClass(User); 
                var person = _basicFunctions.getPerson(User);
                var cl = student.Class;
                var lessons = _context.Lesson.Where(c => c.fk_Class == cl.Id).Include(c => c.Modul).ToList();
                var appointments = _context.Appointment.Where(c => c.fk_Person == person.Id).ToList();
                var classcourse = _context.ClassCourse.Where(c => c.fk_Class == cl.Id).Include(c => c.Course).ToList();
                List<Event> finishedValues = new List<Event>();
                foreach (var lesson in lessons)
                {
                    finishedValues.Add(Event.ConvertLessonToEvent(lesson));
                }
                foreach (var appointment in appointments)
                {
                    finishedValues.Add(Event.ConvertAppointmentToEvent(appointment));
                }
                foreach (var c in classcourse)
                {
                    finishedValues.Add(Event.ConvertCourseToEvent(c.Course));
                }
                return new JsonResult(finishedValues);
            }
            return ApiResponses.GetResponse("");
        }
        public async Task<IActionResult> Generate()
        {
            DataFactory dataFactory = new DataFactory(_context, _rolemanager, _usermanager, _signInManager);
            dataFactory.GenerateLessons(200);
            return ApiResponses.GetResponse("");
        }

        public async Task<IActionResult> GetDataForDownload()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var person = _basicFunctions.getPerson(User);
            var student = _context.Student.Where(s => s.fk_Person == person.Id).Include(s => s.Class).First();
            var cl = student.Class;
            var lessons = _context.Lesson.Where(c => c.fk_Class == cl.Id).Include(c => c.Modul).Include(l => l.ClassRoom).Include(l => l.Teacher).ThenInclude(l => l.Person).ToList();
            var appointments = _context.Appointment.Where(c => c.fk_Person == person.Id).ToList();
            var classcourse = _context.ClassCourse.Where(c => c.fk_Class == cl.Id).Include(c => c.Course).ThenInclude(c => c.modul).ToList();
            DateTime DateStart = DateTime.Now;
            DateTime DateEnd = DateStart.AddMinutes(105);
            string Summary = "Small summary text";
            string Location = "Event location";
            string Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.";
            string FileName = "CalendarItem";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:stackoverflow.com");
            sb.AppendLine("CALSCALE:GREGORIAN");
            sb.AppendLine("METHOD:PUBLISH");
            sb.AppendLine("BEGIN:VTIMEZONE");
            sb.AppendLine("TZID:Europe/Amsterdam");
            sb.AppendLine("BEGIN:STANDARD");
            sb.AppendLine("TZOFFSETTO:+0100");
            sb.AppendLine("TZOFFSETFROM:+0100");
            sb.AppendLine("END:STANDARD");
            sb.AppendLine("END:VTIMEZONE");
            foreach (Lesson lesson in lessons)
            {
                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine("DTSTART:" + lesson.StartTime.ToString("yyyyMMddTHHmm00"));
                sb.AppendLine("DTEND:" + lesson.EndTime.ToString("yyyyMMddTHHmm00"));
                sb.AppendLine("SUMMARY:" + lesson.Modul.Name + "");
                sb.AppendLine("LOCATION:" + lesson.ClassRoom.ToString() + "");
                sb.AppendLine("DESCRIPTION:" + lesson.Teacher.Person.FullName +" "+ lesson.Modul.Name +" "+ lesson.ClassRoom.ToString());
                sb.AppendLine("PRIORITY:3");
                sb.AppendLine("END:VEVENT");
            }
            foreach (ClassCourse course in classcourse)
            {
                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine("DTSTART:" + course.Course.StartTime.ToString("yyyyMMddTHHmm00"));
                sb.AppendLine("DTEND:" + course.Course.EndTime.ToString("yyyyMMddTHHmm00"));
                sb.AppendLine("SUMMARY:" + course.Course.Typ + "");
                sb.AppendLine("LOCATION:");
                sb.AppendLine("DESCRIPTION:" + course.Course.Typ + " vom Modul " + course.Course.modul.Name);
                sb.AppendLine("PRIORITY:3");
                sb.AppendLine("END:VEVENT");
            }
            foreach (Appointment a in appointments)
            {
                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine("DTSTART:" + a.StartTime.ToString("yyyyMMddTHHmm00"));
                sb.AppendLine("DTEND:" + a.EndTime.ToString("yyyyMMddTHHmm00"));
                sb.AppendLine("SUMMARY:" + a.ToShort(a.Reason) + "");
                sb.AppendLine("LOCATION:");
                sb.AppendLine("DESCRIPTION:" + "Sie haben einen Termin wegen " + a.Reason);
                sb.AppendLine("PRIORITY:3");
                sb.AppendLine("END:VEVENT");
            }
            sb.AppendLine("END:VCALENDAR");
            string CalendarItem = sb.ToString();
            System.IO.File.WriteAllText("D:\\Projects\\GudrunDieSiebte\\GudrunDieSiebte\\wwwroot\\clendarfiles\\" + userId+".ics", CalendarItem);
            var path = "/clendarfiles/" + userId + ".ics";
            return ApiResponses.GetResponse(path);
        }






        // GET: Lessons
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Lesson.Include(l => l.Class).ThenInclude(l => l.ClassCourse).Include(l => l.ClassRoom).Include(l => l.Modul).ThenInclude(l => l.Course).Include(l => l.Teacher).Include(l => l.Teacher.Person).ThenInclude(l => l.Appointments).ToListAsync();

            return ApiResponses.GetResponse(_mapper.Map<List<LessonAllDTO>>(applicationDbContext));
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lesson == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            var lesson = await _context.Lesson
                .Include(l => l.Class)
                .Include(l => l.ClassRoom)
                .Include(l => l.Modul)
                .Include(l => l.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            return ApiResponses.GetResponse(lesson);
        }
        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,fk_ClassRoom,fk_Class,fk_Teacher,fk_Modul")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lesson);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            ViewData["fk_Class"] = new SelectList(_context.Class, "Id", "Id", lesson.fk_Class);
            ViewData["fk_ClassRoom"] = new SelectList(_context.ClassRoom, "Id", "Id", lesson.fk_ClassRoom);
            ViewData["fk_Modul"] = new SelectList(_context.Modul, "Id", "Id", lesson.fk_Modul);
            ViewData["fk_Teacher"] = new SelectList(_context.Set<Teacher>(), "Id", "Id", lesson.fk_Teacher);
            return ApiResponses.GetErrorResponse(2, "Not Valid"); ;
        }
        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,fk_ClassRoom,fk_Class,fk_Teacher,fk_Modul")] Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.Id))
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
            ViewData["fk_Class"] = new SelectList(_context.Class, "Id", "Id", lesson.fk_Class);
            ViewData["fk_ClassRoom"] = new SelectList(_context.ClassRoom, "Id", "Id", lesson.fk_ClassRoom);
            ViewData["fk_Modul"] = new SelectList(_context.Modul, "Id", "Id", lesson.fk_Modul);
            ViewData["fk_Teacher"] = new SelectList(_context.Set<Teacher>(), "Id", "Id", lesson.fk_Teacher);
            return ApiResponses.GetErrorResponse(2, "Not Valid");
        }
        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lesson == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Lesson'  is null.");
            }
            var lesson = await _context.Lesson.FindAsync(id);
            if (lesson != null)
            {
                _context.Lesson.Remove(lesson);
            }
            
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }

        private bool LessonExists(int id)
        {
          return _context.Lesson.Any(e => e.Id == id);
        }
    }
}
