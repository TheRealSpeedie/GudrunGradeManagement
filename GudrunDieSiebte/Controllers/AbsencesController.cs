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
using YamlDotNet.Core;
using GudrunDieSiebte.Utility.ObjectClasses;

namespace GudrunDieSiebte.Controllers
{
    public class AbsencesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly BasicFunctions _basicFunctions;

        public AbsencesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _basicFunctions = new BasicFunctions(context);
        }
        private Absence getAbsenceByid(int id)
        {
            var absence = _context.Absence.Where(a => a.Id == id).First();
            return absence;
        }

        private AbsencePerson getAbsencePerson(Person person, Absence absence)
        {
            var absenceperson = _context.AbsencePerson.Where(a => a.fk_Person == person.Id).Where(a => a.fk_Absence == absence.Id).First();
            return absenceperson;
        }
        public async Task<IActionResult> GetAbsencesFromTeacher()
        {
            var person = _basicFunctions.getPerson(User);
            var absences = _context.Absence.Join(_context.AbsenceLesson, absence => absence.Id, absencelesson => absencelesson.fk_Absence, (absence, absencelesson) => new { absence, absencelesson })
                                           .Join(_context.Lesson, absence => absence.absencelesson.fk_Lesson, lesson => lesson.Id, (absence, lesson) => new { absence.absence, absence.absencelesson, lesson })
                                           .Join(_context.Teacher, absence => absence.lesson.fk_Teacher, teacher => teacher.Id, (absence, teacher) => new { absence.absence, absence.absencelesson, absence.lesson, teacher })
                                           .Where(l => l.teacher.Person.Id == person.Id)
                                           .Select(l => l.absence).Include(l => l.Person).ToList();
            List<AbsenceVisibility> allabsences = new List<AbsenceVisibility>();
            foreach (Absence a in absences)
            {
                var absenceperson = _context.AbsencePerson.Where(b => b.fk_Person == person.Id).Where(b => b.fk_Absence == a.Id).First();
                bool visible = false;
                if (absenceperson.isVisible)
                {
                    visible = true;
                }
                AbsenceVisibility absence = new AbsenceVisibility
                {
                    isVisible= visible,
                    Reason = a.Reason,
                    Person = a.Person,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    confirmed = a.confirmed,
                    Id = a.Id
                };
                allabsences.Add(absence);
            }

            return ApiResponses.GetResponse((_mapper.Map<List<AbsenceAllVisibleDTO>>(allabsences)));
        }
		public async Task<IActionResult> ShowDetails(int id)
		{
			var absence = _context.Absence.Where(a => a.Id == id).Include(a => a.Person).First();
            var lessons = _context.Lesson.Join(_context.AbsenceLesson, lesson => lesson.Id, absencelesson => absencelesson.fk_Lesson, (lesson, absencelesson) => new { lesson, absencelesson })
                                        .Where(a => a.absencelesson.fk_Absence == absence.Id)
                                        .Select(a => a.lesson).Include(a => a.Modul).ToList();

			var persons = _context.Person.Join(_context.AbsencePerson, person => person.Id, absencePerson => absencePerson.fk_Person, (person, absencePerson) => new { person, absencePerson })
                                         .Where(a => a.absencePerson.fk_Absence == absence.Id)
                                         .Select(a => a.person).ToList(); 

			AbsenceWithListLesson AbsenceListLesson = new AbsenceWithListLesson
            {
                Absence = absence,
                Lessons = lessons,
				Persons = persons
			};
			return ApiResponses.GetResponse((_mapper.Map<AbsenceWithListLessonDTO>(AbsenceListLesson)));
		}
		public async Task<IActionResult> DontShow(int id)
        {
            var absence = getAbsenceByid(id);
            var person = _basicFunctions.getPerson(User);
            var absenceperson = getAbsencePerson(person, absence);
            absenceperson.isVisible = false;
            _context.SaveChanges();
            return ApiResponses.GetResponse("");
        }
 
        public async Task<IActionResult> Show(int id)
        {
            var absence = getAbsenceByid(id);
            var person = _basicFunctions.getPerson(User);
            var absenceperson = getAbsencePerson(person, absence);
            absenceperson.isVisible = true;
            _context.SaveChanges();
            return ApiResponses.GetResponse("");
        }
        public async Task<IActionResult> Confirm(int id)
        {
            var absence = getAbsenceByid(id);
            absence.confirmed = true;
            _context.SaveChanges();
            return ApiResponses.GetResponse("");
        }
        
        public async Task<IActionResult> UnConfirm(int id)
        {
            var absence = getAbsenceByid(id);
            absence.confirmed = false;
            _context.SaveChanges();
            return ApiResponses.GetResponse("");
        }






        // GET: Absences
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Absence.Include(a => a.Person).ToListAsync();
            return ApiResponses.GetResponse(_mapper.Map<List<AbsenceAllDTO>>(applicationDbContext));
        }
        // GET: Absences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Absence == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            var absence = await _context.Absence
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (absence == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");

            }

            return ApiResponses.GetResponse(absence);
        }
        // POST: Absences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Reason,fk_Person")] Absence absence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(absence);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            ViewData["fk_Person"] = new SelectList(_context.Person, "Id", "Id", absence.fk_Person);
            return ApiResponses.GetErrorResponse(2, "Not Valid");

        }
        // POST: Absences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Reason,fk_Person")] Absence absence)
        {
            if (id != absence.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(absence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbsenceExists(absence.Id))
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
            ViewData["fk_Person"] = new SelectList(_context.Person, "Id", "Id", absence.fk_Person);
            return ApiResponses.GetErrorResponse(2, "Not Valid");

        }


        // POST: Absences/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Absence == null)
            {
                return ApiResponses.GetErrorResponse(0, "Entity set 'Student'  is null.");
            }
            var absence = await _context.Absence.FindAsync(id);
            if (absence != null)
            {
                _context.Absence.Remove(absence);
            }
            
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }

        private bool AbsenceExists(int id)
        {
          return _context.Absence.Any(e => e.Id == id);
        }
    }
}
