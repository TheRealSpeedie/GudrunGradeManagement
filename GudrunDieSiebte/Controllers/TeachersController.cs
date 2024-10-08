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
    public class TeachersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly BasicFunctions _basicFunctions;
        public TeachersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _basicFunctions = new BasicFunctions(context);
        }
        public async Task<IActionResult> GetMyTeachers()
        {
            var student = _basicFunctions.getStudentWithClass(User); 
            var classe = student.Class;
            var lessons = _context.Lesson.Where(l => l.fk_Class == classe.Id).ToList();
            List<Lesson> mylesson = new List<Lesson>();
            foreach(Lesson l in lessons)
            {
                _basicFunctions.AddToListIfNotInList<Lesson>(mylesson, l);
            }
            List<Teacher> teacherspace = new List<Teacher>();
            foreach(var l in mylesson)
            {
                var t = _context.Lesson.Where(s => s.Id == l.Id).Include( s => s.Teacher).ThenInclude( s => s.Person).Include(s => s.Teacher).ThenInclude(s => s.SchoolManagement).Select(s => s.Teacher).First();
                teacherspace.Add(t);
            }
            List<Teacher> myteachers = new List<Teacher>();
            foreach(var t in teacherspace)
            {
                _basicFunctions.AddToListIfNotInList(myteachers, t);
            }
            return ApiResponses.GetResponse(_mapper.Map<List<TeacherSchoolManagementDTO>>(myteachers));
        }
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = _context.Teacher.Include(t => t.SchoolManagement).Include(t => t.Person).ToList();
            return ApiResponses.GetResponse(_mapper.Map<List<TeacherSchoolManagementDTO>>(teachers));
        }




        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Teacher.Include(t => t.Person).Include(t => t.SchoolManagement).ToListAsync();

            return ApiResponses.GetResponse(_mapper.Map<List<TeacherSchoolManagementDTO>>(applicationDbContext));
        }
        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teacher == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            var teacher = await _context.Teacher
                .Include(t => t.Person)
                .Include(t => t.SchoolManagement)
                .Include(t => t.Lessons).ThenInclude(t => t.Class)
                .Include(t => t.Lessons).ThenInclude(t => t.Modul)
                .Include(t => t.Notifications)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            return ApiResponses.GetResponse(_mapper.Map<TeacherAllDTO>(teacher));
        }
        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HireDate,Id,LastName,FirstName,Birthday,Address,City")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            return ApiResponses.GetErrorResponse(2, "Not Valid");
        }
        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HireDate,Id,LastName,FirstName,Birthday,Address,City")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teacher == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Teacher'  is null.");
            }
            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher != null)
            {
                _context.Teacher.Remove(teacher);
            }
            
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }
        private bool TeacherExists(int id)
        {
          return _context.Teacher.Any(e => e.Id == id);
        }
    }
}
