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
using NuGet.DependencyResolver;
using System.Reflection;
using YamlDotNet.Core;

namespace GudrunDieSiebte.Controllers
{
    public class ModulsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly BasicFunctions _basicFunctions;

        public ModulsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _basicFunctions = new BasicFunctions(context);
        }

        private List<Modul> getModulsFromLessonList(List<Modul> moduls, List<Lesson> lessons)
        {
            foreach (var c in lessons)
            {
                _basicFunctions.AddToListIfNotInList(moduls, c.Modul);
            }
            return moduls;
        }
        private List<Modul> getModulsFromStudent()
        {
            var student = _basicFunctions.getStudentWithClass(User);
            var cl = student.Class;
            var lessons = _context.Lesson.Where(l => l.fk_Class == cl.Id).Include(l => l.Modul).ThenInclude(l => l.Exam).ThenInclude(l => l.grades).ToList();
            List<Modul> myModuls = new List<Modul>();
            getModulsFromLessonList(myModuls, lessons);
            return myModuls;
        }
        public async Task<IActionResult> GetMyGrades()
        {
            var student = _basicFunctions.getStudent(User);
            var moduls = getModulsFromStudent();
            foreach (Modul m in moduls)
            {
                List<Exam> examsFromModul = new List<Exam>();
                examsFromModul.AddRange(_context.Exam.Where(e => e.fk_Modul == m.Id).ToList());
                examsFromModul = _basicFunctions.checkForDuplicate(examsFromModul);
                List<Exam> myExamsWithGrades = new List<Exam>();
                foreach (Exam e in examsFromModul)
                {
                    var grades = _context.Grade.Where(g => g.fk_Exam == e.Id).Where(g => g.fk_Student == student.Id).ToList();
                    Exam myExam = new Exam
                    {
                        Id = e.Id,
                        Highscore = e.Highscore,
                        Examday = e.Examday,
                        name = e.name,
                        Thema = e.Thema,
                        fk_Modul = e.fk_Modul,
                        grades = grades
                    };
                    myExamsWithGrades.Add(myExam);
                }
                m.Exam = myExamsWithGrades;
            }
            
            return ApiResponses.GetResponse(_mapper.Map<List<ModulExamDTO>>(moduls));
        }
        [HttpGet("Moduls/getModuleByName/{name}")]
        public async Task<IActionResult> getModuleByName(string name)
        {
            var modul = _context.Modul.Where(m => m.Name == name).First();
            return ApiResponses.GetResponse(_mapper.Map<ModulDTO>(modul));
        }

        public async Task<IActionResult> GetMyModuls()
        {
            List<Modul> myModuls = getModulsFromStudent();
            return ApiResponses.GetResponse(_mapper.Map<List<ModulDTO>>(myModuls));
        }
        public async Task<IActionResult> getAllModule()
        {
            var moduls = _context.Modul.ToList();
            return ApiResponses.GetResponse(_mapper.Map<List<ModulDTO>>(moduls));
        }
        public async Task<IActionResult> GetShedule(int id)
        {
            var modul = _context.Modul.Where(m => m.Id == id).Include(m => m.Lessons).Include(m => m.Exam).ToList();
            return ApiResponses.GetResponse(_mapper.Map<List<ModulLessonExamDTO>>(modul));
        }








        // GET: Moduls
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Modul.Include(l => l.Lessons).Include(l => l.Exam).Include(l => l.Notification).Include(l => l.Course).ToListAsync();

            return ApiResponses.GetResponse(_mapper.Map<List<ModuleAllDTO>>(applicationDbContext));
        }
        // GET: Moduls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Modul == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            var modul = await _context.Modul
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modul == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            return ApiResponses.GetResponse(modul);
        }
        // POST: Moduls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Modul modul)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modul);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            return ApiResponses.GetErrorResponse(2, "Not Valid");
        }

        // POST: Moduls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Modul modul)
        {
            if (id != modul.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modul);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModulExists(modul.Id))
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
        // POST: Moduls/Delete/5
        [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Modul == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Modul'  is null.");
            }
            var modul = await _context.Modul.FindAsync(id);
            if (modul != null)
            {
                _context.Modul.Remove(modul);
            }

            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }

        private bool ModulExists(int id)
        {
            return _context.Modul.Any(e => e.Id == id);
        }
    }
}
