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
using NuGet.Packaging;
using NuGet.DependencyResolver;
using System.Reflection;
using GudrunDieSiebte.Utility.ObjectClasses;

namespace GudrunDieSiebte.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly BasicFunctions _basicFunctions;

        public ClassesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _basicFunctions = new BasicFunctions(context);
        }
        private List<Modul> getModulsFromClass(Class classe)
        {
            var modulslist = _context.Modul.Join(_context.Lesson, modul => modul.Id, lesson => lesson.fk_Modul, (modul, lesson) => new { modul, lesson })
                                      .Join(_context.Class, modul => modul.lesson.fk_Class, classe => classe.Id, (modul, classe) => new { modul.modul, modul.lesson, classe })
                                      .Where(l => l.classe.Id == classe.Id)
                                      .Select(i => i.modul).ToList();
            List<Modul> modules = new List<Modul>();
            foreach (var m in modulslist)
            {
                _basicFunctions.AddToListIfNotInList(modules, m);
            }
            return modules;
        }
        private float getExamsWithClassAndModul(Class c, Modul m)
        {
            var exams = _context.Exam.Join(_context.Grade, exam => exam.Id, Grade => Grade.fk_Exam, (exam, Grade) => new { exam, Grade })
                                            .Join(_context.Student, exam => exam.Grade.fk_Student, student => student.Id, (exam, student) => new { exam.exam, exam.Grade, student })
                                            .Where(e => e.Grade.Student.fk_Class == c.Id)
                                            .Where(e => e.exam.fk_Modul == m.Id)
                                            .Select(e => e.Grade.grade).ToList();
            float average = 0;
            if (exams.Count != 0)
            {
                average = exams.Average();
            }
            return average;
        }
        private List<Class> getClassFromTeacher(Teacher teacher)
        {
            var classes1 = _context.Class.Join(_context.Lesson, classe => classe.Id, lesson => lesson.fk_Class, (classe, lesson) => new { classe, lesson })
                                          .Where(c => c.lesson.fk_Teacher == teacher.Id).Select(c => c.classe).ToList();
            List<Class> classes2 = new List<Class>();
            foreach (var c in classes1)
            {
                _basicFunctions.AddToListIfNotInList(classes2, c);
            }
            return classes2;
        }
        public async Task<IActionResult> getClassesWithStudentTeacherModul()
        {
            var classes = _context.Class.Include(c => c.Teacher).ThenInclude(c => c.Person).Include(c => c.Students).ThenInclude(c => c.Person).ToList();
            List<ClassTeacherStudentsModuls> retunClasses = new List<ClassTeacherStudentsModuls>();
            foreach (Class c in classes)
            {
                var classTeacher = c.Teacher.Person.FullName;
                var moduls = getModulsFromClass(c);
                List<Modulname> modulnames = new List<Modulname>();
                List<Studentname> studentnames = new List<Studentname>();
                foreach (Modul m in moduls)
                {
                    modulnames.Add(new Modulname { modulname = m.Name });
                }
                foreach (Student student in c.Students)
                {
                    studentnames.Add(new Studentname { studentname = student.Person.FullName });

                }
                var ClassTeacherStudentModuls = new ClassTeacherStudentsModuls { Id = c.Id, classname = c.Name, teachername = classTeacher, Moduls = modulnames, Students = studentnames };
                retunClasses.Add(ClassTeacherStudentModuls);
            }
            return ApiResponses.GetResponse(_mapper.Map<List<ClassTeacherStudentsModulsDTO>>(retunClasses));
        }
        public async Task<IActionResult> getGradesModulsClass()
        {
            var teacher = _basicFunctions.getTeacher(User);
            List<Class> classes1 = getClassFromTeacher(teacher);
            List<Class> classes = new List<Class>();
            foreach (var c in classes1)
            {
                _basicFunctions.AddToListIfNotInList(classes, c);
            }
            List<ModulClassExam> moduls = new List<ModulClassExam>();
            foreach (var c in classes)
            {
                List<Modul> modules = getModulsFromClass(c);
                List<ModulnameAverage> names = new List<ModulnameAverage>();
                foreach (var m in modules)
                {
                    float average = getExamsWithClassAndModul(c, m);
                    var modulsname = new ModulnameAverage { modulname = m.Name, average = average };
                    names.Add(modulsname);
                }
                var modulClassExam = new ModulClassExam { id = c.Id, classname = c.Name, moduls = names };
                moduls.Add(modulClassExam);
            }
            return ApiResponses.GetResponse(_mapper.Map<List<ModulClassExamDTO>>(moduls));
        }

        public async Task<IActionResult> getModulsByClass(int id)
        {
            var teacher = _basicFunctions.getTeacher(User);
            var classe = _context.Class.Where(t => t.Id == id).First();
            List<Modul> modules = getModulsFromClass(classe);
            List<ModulnameAverage> names = new List<ModulnameAverage>();
            foreach (var m in modules)
            {
                float average = getExamsWithClassAndModul(classe, m);
                var modulsname = new ModulnameAverage { modulname = m.Name, average = average };
                names.Add(modulsname);
            }
            var dings = new ModulClassExam { id = classe.Id, classname = classe.Name, moduls = names };

            return ApiResponses.GetResponse(_mapper.Map<ModulClassExamDTO>(dings));
        }
        [HttpGet("Classes/getStudentsByClassName/{name}")]
        public async Task<IActionResult> getStudentsByClassName(string name)
        {
            var classe = _context.Class.Where(c => c.Name == name).First();
            var students = _context.Student.Where(s => s.fk_Class == classe.Id).Include(s => s.Person).ToList();
            return ApiResponses.GetResponse(_mapper.Map<List<StudentDTO>>(students));
        }
        public async Task<IActionResult> getAllClassesFromTeacher()
        {
            var teacher = _basicFunctions.getTeacher(User);
            var classe = getClassFromTeacher(teacher);
            return ApiResponses.GetResponse(_mapper.Map<List<ClassDTO>>(classe));
        }
        public List<Exam> getexamListByModulClass(Class c, Modul m)
        {
            var exams = _context.Exam.Join(_context.Grade, exam => exam.Id, grade => grade.fk_Exam, (exam, grade) => new { exam, grade })
                                    .Join(_context.Student, exam => exam.grade.fk_Student, student => student.Id, (exam, student) => new { exam, student })
                                    .Where(s => s.student.fk_Class == c.Id)
                                    .Where(e => e.exam.exam.fk_Modul == m.Id)
                                    .Select(x => x.exam.exam).Include(x => x.grades).ToList();
            List<Exam> exames = new List<Exam>();
            foreach (var exam in exams)
            {
                _basicFunctions.AddToListIfNotInList(exames, exam);
            }
            return exames;
        }
        [HttpGet("/Classes/GetModulbyIDWithClass/{modulID}/{classID}")]
        public async Task<IActionResult> GetModulbyIDWithClass(int modulID, int classID)
        {
            var teacher = _basicFunctions.getTeacher(User);
            var modul = _context.Modul.Where(m => m.Id == modulID).First();
            var c = _context.Class.Where(l => l.Id == classID).First();
            var exames = getexamListByModulClass(c, modul);
            List<examWithAverageFromClass> examlist = new List<examWithAverageFromClass>();
            foreach (var exam in exames)
            {
                var average = getAverageByExamID(exam.Id, classID);
                examWithAverageFromClass examitem = new examWithAverageFromClass
                {
                    Id = exam.Id,
                    name = exam.name,
                    thema = exam.Thema,
                    Examday = exam.Examday,
                    ClassSpecificAverage = average
                };
                examlist.Add(examitem);
            }
            ModulWithClassExams mymodul = new ModulWithClassExams
            {
                Id = modul.Id,
                Name = modul.Name,
                description = modul.description,
                Shedule = modul.Shedule,
                exams = examlist
            };
            return ApiResponses.GetResponse(_mapper.Map<ModulWithClassExamsDTO>(mymodul));
        }
        private double getAverageByExamID(int examID, int classID)
        {
            var exam = _context.Exam.Where(e => e.Id == examID).First();
            var grades = _context.Grade.Join(_context.Student, grade => grade.fk_Student, student => student.Id, (grade, student) => new { grade, student })
                                    .Where(s => s.student.fk_Class == classID).Where(e => e.grade.fk_Exam == exam.Id).Select(x => x.grade).ToList();
            double average = 0.0;
            if (grades != null)
            {
                foreach (Grade grade in grades)
                {
                    average += grade.grade;
                }
                average = average / grades.Count;
            }
            return average;
        }
        private List<Modul> getModulsFromClass(Class c, Teacher teacher)
        {
            var modules = _context.Modul.Join(_context.Lesson, modul => modul.Id, lesson => lesson.fk_Modul, (modul, lesson) => new { modul, lesson })
                                          .Where(l => l.lesson.fk_Class == c.Id)
                                          .Where(l => l.lesson.fk_Teacher == teacher.Id)
                                          .Select(l => l.modul).ToList();
            List<Modul> Mymodul = new List<Modul>();
            foreach (Modul modul in modules)
            {
                _basicFunctions.AddToListIfNotInList<Modul>(Mymodul, modul);
            }
            return Mymodul;
        }
        [HttpGet("/Classes/getModulsFromClassByID/{classID}")]
        public async Task<IActionResult> getModulsFromClassByID(int classID)
        {
            var teacher = _basicFunctions.getTeacher(User);
            var c = _context.Class.Where(c => c.Id == classID).First();
            var modules = getModulsFromClass(c, teacher);
            ClassModul classModul = new ClassModul
            {
                Id = c.Id,
                Name = c.Name,
                moduls = modules
            };
            return ApiResponses.GetResponse(_mapper.Map<ClassModulDTO>(classModul));
        }
        public async Task<IActionResult> getClassesModulExam()
        {
            var teacher = _basicFunctions.getTeacher(User);
            var classes = getClassFromTeacher(teacher);
            List<ClassModulExamList> myClassModulExamList = new List<ClassModulExamList>();
            foreach (var c in classes)
            {
                var modules = getModulsFromClass(c, teacher);
                List<Modul> moduls = new List<Modul>();
                foreach (Modul m in modules)
                {
                    _basicFunctions.AddToListIfNotInList(moduls, m);
                }
                foreach (Modul m in moduls)
                {
                    var exames = getexamListByModulClass(c, m);
                    m.Exam = exames;
                }
                ClassModulExamList myobject = new ClassModulExamList { classe = c, moduls = moduls };
                myClassModulExamList.Add(myobject);
            }
            return ApiResponses.GetResponse(_mapper.Map<List<ClassModulExamListDTO>>(myClassModulExamList));
        }










        // GET: Classes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Class.Include(t => t.Students).ThenInclude(t => t.Person).Include(t => t.Lessons).ThenInclude(t => t.Modul).Include(t => t.Teacher).ThenInclude(t => t.Person).ToListAsync();

            return ApiResponses.GetResponse(_mapper.Map<List<ClassAllDTO>>(applicationDbContext));
        }
        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Class == null)
            {
                return ApiResponses.GetErrorResponse(1, "not Found");
            }

            var @class = await _context.Class
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@class == null)
            {
                return ApiResponses.GetErrorResponse(1, "not Found");
            }

            return ApiResponses.GetResponse(@class);
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name")] Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(@class);
            }
            return ApiResponses.GetErrorResponse(2, "not valid"); ; //invalide Botschaft
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Class @class)
        {
            if (id != @class.Id)
            {
                return ApiResponses.GetErrorResponse(1, "not Found");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.Id))
                    {
                        return ApiResponses.GetErrorResponse(1, "not Found");
                    }
                    else
                    {
                        throw;
                    }
                }
                return ApiResponses.GetResponse(@class);
            }
            return ApiResponses.GetErrorResponse(2, "not valid");
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        // TODO: [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Class == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Class'  is null.");
            }
            var @class = await _context.Class.FindAsync(id);
            if (@class != null)
            {
                _context.Class.Remove(@class);
            }

            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }

        private bool ClassExists(int id)
        {
            return _context.Class.Any(e => e.Id == id);
        }
    }
}
