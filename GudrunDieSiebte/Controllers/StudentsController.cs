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
using System.Diagnostics;
using AutoMapper;
using GudrunDieSiebte.DTO;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using GudrunDieSiebte.Utility.ObjectClasses;
using System.Transactions;

namespace GudrunDieSiebte.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly BasicFunctions _basicFunctions;
        public StudentsController(ApplicationDbContext context, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _mapper = mapper;
            _rolemanager = roleManager;
            _usermanager = userManager;
            _signInManager = signInManager;
            _basicFunctions = new BasicFunctions(context);
        }
        private void ChangeDone(int TodoId, bool isdone)
        {
            var student = _basicFunctions.getStudent(User);
            var todo = _context.StudentToDo.Where(c => c.fk_ToDo == TodoId).Where(c => c.fk_Student == student.Id).First();
            todo.Done = isdone;
            _context.SaveChanges();
        }
        [HttpGet]
        private List<ToDoWithConfirm> GetMyToDosWithConfirm()
        {
            var student = _basicFunctions.getStudent(User, "Class");
            var cl = student.Class;
            var todos = _context.ToDo
                .Include(x => x.Modul)
                .Include(x => x.Teacher)
                .ThenInclude(x => x.Person)
                .Join(_context.StudentToDo, toDo => toDo.Id, studentTodo => studentTodo.fk_ToDo, (toDo, studentTodo) => new { toDo, studentTodo })
                .Where(x => x.toDo.fk_Class == cl.Id)
                .Where(x => x.studentTodo.fk_Student == student.Id)
                .Select(x => new { x.toDo, x.studentTodo.Done })
                .ToList();
            List<ToDoWithConfirm> listOfToDosWithConfirm = new List<ToDoWithConfirm>();
            todos = _basicFunctions.checkForDuplicate(todos);
            foreach (var t in todos)
            {
                ToDoWithConfirm todoWithConfirm = new ToDoWithConfirm
                {
                    Id = t.toDo.Id,
                    Task = t.toDo.Task,
                    UpTo = t.toDo.UpTo,
                    Modul = t.toDo.Modul,
                    Teacher = t.toDo.Teacher,
                    Class = t.toDo.Class,
                    Done = t.Done
                };
                listOfToDosWithConfirm.Add(todoWithConfirm);
            }
            return listOfToDosWithConfirm;
        }

        public async Task<IActionResult> Generate()
        {
            DataFactory dataFactory = new DataFactory(_context, _rolemanager, _usermanager, _signInManager);
            await dataFactory.GenerateData();
            return ApiResponses.GetResponse(new { Link = "https://localhost:7138/Home/Users", History = dataFactory.GetHistory() });
        }
        public async Task<IActionResult> GetClassmates()
        {
            var student = _basicFunctions.getStudent(User, "Class");
            var cl = student.Class;
            var classmates =  _context.Student.Where(c => c.fk_Class == cl.Id).Include(c => c.Person).ToList();
            return ApiResponses.GetResponse(_mapper.Map<List<Student_ClassDTO>>(classmates));
        }
        public async Task<IActionResult> GetMyToDos()
        {
            var todos = GetMyToDosWithConfirm();
            return ApiResponses.GetResponse(_mapper.Map<List<ToDoWithConfirmDTO>>(todos));
        }
        public async Task<IActionResult> SetDone(int id)
        {
            ChangeDone(id, true);
            return ApiResponses.GetResponse("");
        }
        public async Task<IActionResult> ResetDone(int id)
        {
            ChangeDone(id, false);
            return ApiResponses.GetResponse("");
        }
       




        // GET: Students
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Student.Include(s => s.Class).Include(s => s.Person).Include(s => s.TeachingCompany).ToListAsync();

            return ApiResponses.GetResponse(_mapper.Map<List<StudentAllDTO>>(applicationDbContext));

        }
        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            var student = await _context.Student
                .Include(s => s.Class)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            return ApiResponses.GetResponse(student);
        }
        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentDate,fk_Class,Id,LastName,FirstName,Birthday,Address,City")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            ViewData["fk_Class"] = new SelectList(_context.Class, "Id", "Id", student.fk_Class);
            return ApiResponses.GetErrorResponse(2, "Not Valid");
        }
        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentDate,fk_Class,Id,LastName,FirstName,Birthday,Address,City")] Student student)
        {
            if (id != student.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["fk_Class"] = new SelectList(_context.Class, "Id", "Id", student.fk_Class);
            return ApiResponses.GetErrorResponse(2, "Not Valid");
        }
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        // TODO: [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Student == null)
            {
                return ApiResponses.GetErrorResponse(0, "Entity set 'Student'  is null.");
            }
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse("Delete success");
        }

        private bool StudentExists(int id)
        {
          return _context.Student.Any(e => e.Id == id);
        }
    }
}
