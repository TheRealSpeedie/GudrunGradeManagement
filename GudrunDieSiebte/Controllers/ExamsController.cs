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
using CsvHelper;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using OpenHtmlToPdf;
using System.Drawing;
using System.IO;
using PdfSharp;
using Aspose.Pdf;
using Aspose.Pdf.Operators;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.SignalR;
using NuGet.DependencyResolver;
using YamlDotNet.Core.Tokens;
using System.Collections.Immutable;
using GudrunDieSiebte.Utility.ObjectClasses;
using Newtonsoft.Json;

namespace GudrunDieSiebte.Controllers
{
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly BasicFunctions _basicFunctions;

        public ExamsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _basicFunctions = new BasicFunctions(context);
        }
        private List<Exam> getExamsWithModulAndStudent(int modulid, Student student)
        {
            var exams = _context.Exam.Join(_context.Grade, exam => exam.Id, Grade => Grade.fk_Exam, (exam, Grade) => new { exam, Grade })
                                      .Join(_context.Student, exam => exam.Grade.fk_Student, student => student.Id, (exam, student) => new { exam.exam, exam.Grade, student })
                                       .Where(g => g.exam.fk_Modul == modulid).Where(g => g.Grade.fk_Student == student.Id).Select(e => e.exam).ToList();
            return exams;
        }
        private bool ExamExists(int id)
        {
            return _context.Exam.Any(e => e.Id == id);
        }
        private float mathAverage(List<Exam> exams)
        {
            float value = 0;
            foreach (var e in exams)
            {
                var g = _context.Grade.Where(g => g.fk_Exam == e.Id).First();
                value += g.grade;
            }
            float average = value / exams.Count();
            return average;
        }
        private List<Student> getListStudentsWithGrade(Class classe, Exam exam)
        {
            var students = _context.Student.Where(s => s.fk_Class == classe.Id).Include(s => s.Grades).Include(s => s.Person).ToList();
            foreach (Student s in students)
            {
                var currentGrade = _context.Grade.Join(_context.Student, grade => grade.fk_Student, student => student.Id, (grade, student) => new { grade, student })
                   .Where(g => g.grade.fk_Exam == exam.Id).Where(g => g.student.Id == s.Id).Select(g => g.grade).FirstOrDefault();
                List<Grade> myGradeList = new List<Grade>();
                Grade mygrade = new Grade();
                if (currentGrade != null)
                {
                    mygrade.Id = currentGrade.Id;
                    mygrade.grade = currentGrade.grade;
                    mygrade.Score = currentGrade.Score;
                    mygrade.isConfirmed = currentGrade.isConfirmed;
                }
                else
                {
                    mygrade.Id = 0;
                    mygrade.grade = 0;
                    mygrade.Score = 0;
                    mygrade.isConfirmed = false;
                }
                myGradeList.Add(mygrade);
                s.Grades = myGradeList;
            }
            return students;
        }
        public async Task<IActionResult> GetAverage(int id, bool ret = false)
        {
            var student = _basicFunctions.getStudent(User);
            var exams = getExamsWithModulAndStudent(id, student);
            var value = mathAverage(exams);
            return ApiResponses.GetResponse(value);
        }
        public async Task<IActionResult> GetPointsFromModul(int id)
        {
            var student = _basicFunctions.getStudent(User);
            var m = getExamsWithModulAndStudent(id, student);
            float avg = 0;
            if (m.Count > 0)
            {
                avg = mathAverage(m);
                avg *= 2;
                avg = (float)Math.Round(avg, MidpointRounding.ToEven);
                avg /= 2;
            }
            var points = checkpoints(avg);
            return ApiResponses.GetResponse(points);
        }
        public double checkpoints(float avg)
        {
            string myString = avg.ToString();
            if (myString.Length < 4)
            {
                if (myString.Length == 1)
                {
                    myString = myString + ".00";
                }
                else if (myString.Length == 3)
                {
                    myString = myString + "0";
                }
            }
            string part1 = myString.Substring(0, 1);
            string part2 = myString.Substring(2, 2);
            double resultpart1 = 0;
            double resultpart2 = 0;
            switch (part1)
            {
                case "1":
                    resultpart1 -= 6;
                    break;
                case "2":
                    resultpart1 -= 4;
                    break;
                case "3":
                    resultpart1 -= 2;
                    break;
                case "4":
                    resultpart1 += 0;
                    break;
                case "5":
                    resultpart1 += 2;
                    break;
                case "6":
                    resultpart1 += 4;
                    break;
            }
            int parsedPart2 = int.Parse(part2);
            if (parsedPart2 < 12)
            {
                resultpart2 += 0;
            }
            if (parsedPart2 > 13 && parsedPart2 < 37)
            {
                resultpart2 += 0.5;
            }
            if (parsedPart2 > 38 && parsedPart2 < 62)
            {
                resultpart2 += 1;
            }
            if (parsedPart2 > 63 && parsedPart2 < 87)
            {
                resultpart2 += 1.5;
            }
            if (parsedPart2 > 87)
            {
                resultpart2 += 2;
            }
            double result = resultpart1 + resultpart2;
            return result;
        }
        public async Task<IActionResult> IsConfirm(int id)
        {
            var student = _basicFunctions.getStudent(User);
            var exams = getExamsWithModulAndStudent(id, student);
            string confirm = "";
            if (exams.Count > 0)
            {
                confirm = "true";
                foreach (var e in exams)
                {
                    var grade = _context.Grade.Where(g => g.fk_Exam == e.Id).First();

                    if (!grade.isConfirmed)
                    {
                        confirm = "false";
                        break;
                    }
                }
            }
            else
            {
                confirm = "undefiniert";
            }
            return ApiResponses.GetResponse(confirm);
        }
        public async Task<IActionResult> SetConfirmAll(int id)
        {
            var student = _basicFunctions.getStudent(User);
            var exams = getExamsWithModulAndStudent(id, student);
            foreach (var e in exams)
            {
                var grade = _context.Grade.Where(g => g.fk_Exam == e.Id).First();
                grade.isConfirmed = true;
                _context.SaveChanges();
            }
            return ApiResponses.GetResponse(_mapper.Map<List<ExamDTO>>(exams));
        }
        public async Task<IActionResult> SetConfirm(int id)
        {
            var exam = _context.Exam.Where(g => g.Id == id).First();
            var grade = _context.Grade.Where(g => g.fk_Exam == exam.Id).First();
            grade.isConfirmed = true;
            return ApiResponses.GetResponse(exam);
        }
        public async Task<IActionResult> GetCSVTestimony()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = _basicFunctions.getStudent(User);
            var cl = student.Class;
            var classcourse = _context.ClassCourse.Where(c => c.fk_Class == cl.Id).Include(c => c.Course).ThenInclude(c => c.modul).ThenInclude(c => c.Exam).ToList();
            List<Modul> moduls = new List<Modul>();
            foreach (var c in classcourse)
            {
                if (!moduls.Contains(c.Course.modul))
                {
                    moduls.Add(c.Course.modul);
                }
            }
            var records = new List<Testimony>();
            foreach (var m in moduls)
            {
                var exams = getExamsWithModulAndStudent(m.Id, student);
                float avg = mathAverage(exams);
                avg *= 2;
                avg = (float)Math.Round(avg, MidpointRounding.ToEven);
                avg /= 2;
                records.Add(new Testimony { modul = m.Name, grade = avg });
            }
            using (var writer = new StreamWriter("C:\\Users\\akoesters\\Desktop\\Schule\\GudrunDieSiebte\\GudrunDieSiebte\\wwwroot\\csvfiles\\" + userId + ".csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
            var path = "/csvfiles/" + userId + ".csv";
            return ApiResponses.GetResponse(path);
        }

        public async Task<IActionResult> GetPDFTestimony()
        {
            var student = _basicFunctions.getStudent(User);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Person.Where(u => u.fk_user == userId).First();
            var cl = student.Class;
            var classcourse = _context.ClassCourse.Where(c => c.fk_Class == cl.Id).Include(c => c.Course).ThenInclude(c => c.modul).ThenInclude(c => c.Exam).ToList();
            List<Modul> moduls = new List<Modul>();
            foreach (var c in classcourse)
            {
                if (!moduls.Contains(c.Course.modul))
                {
                    moduls.Add(c.Course.modul);
                }
            }
            Document document = new Document();
            Page page = document.Pages.Add();
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Zeugnis von " + user.FullName));
            Aspose.Pdf.Table table = new Aspose.Pdf.Table();
            table.Border = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.FromRgb(System.Drawing.Color.LightGray));
            table.DefaultCellBorder = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, .5f, Aspose.Pdf.Color.FromRgb(System.Drawing.Color.LightGray));
            Row row = table.Rows.Add();
            row.Cells.Add("Modul");
            row.Cells.Add("Note");
            for (int x = 0; x < moduls.Count; x++)
            {
                Row row2 = table.Rows.Add();
                var exams = getExamsWithModulAndStudent(moduls[x].Id, student);
                float avg = mathAverage(exams);
                avg *= 2;
                avg = (float)Math.Round(avg, MidpointRounding.ToEven);
                avg /= 2;
                row2.Cells.Add(moduls[x].Name);
                row2.Cells.Add(avg.ToString());
            }
            document.Pages[1].Paragraphs.Add(table);
            var path = "C:\\Users\\akoesters\\Desktop\\Schule\\GudrunDieSiebte\\GudrunDieSiebte\\wwwroot\\pdffiles\\" + userId + ".pdf";
            document.Save(path);
            return ApiResponses.GetResponse(path);
        }
        public async Task<IActionResult> GetListexams(int id)
        {
            var student = _basicFunctions.getStudent(User);
            var exams = getExamsWithModulAndStudent(id, student);
            foreach (var e in exams)
            {
                var grade = _context.Grade.Where(g => g.fk_Student == student.Id).Where(g => g.fk_Exam == e.Id).ToList();
                e.grades = grade;
            }
            return ApiResponses.GetResponse(_mapper.Map<List<ExamGradeDTO>>(exams));
        }
        public async Task<IActionResult> DetailExam(int id)
        {
            var exam = _context.Exam.Where(g => g.Id == id).Include(g => g.grades).ThenInclude(g => g.Student).ThenInclude(g => g.Person).First();
            return ApiResponses.GetResponse(_mapper.Map<ExamAllDTO>(exam));
        }
       
        [HttpGet("/Exams/getAllStudentsWithExam/{examID}/{classID}")]
        public async Task<IActionResult> getAllStudentsWithExam(int examID, int classID)
        {
            var exam =  _context.Exam.Where(g => g.Id == examID).First();
            var classe = _context.Class.Where(c => c.Id == classID).First();
            var students = getListStudentsWithGrade(classe, exam);
            ClassWithStudentsGrades myClass = new ClassWithStudentsGrades
            {
                Id = classe.Id,
                Name = classe.Name,
                myStudents = students,
                Highscore = exam.Highscore
            };
            return ApiResponses.GetResponse(_mapper.Map<ClassWithStudentsGradesDTO>(myClass));
        }
        [HttpGet("/Exams/getExamsFromClass/{examID}/{classID}")]
        public async Task<IActionResult> getExamsFromClass(int examID, int classID)
        {
            var exam = _context.Exam.Where(g => g.Id == examID).First();
            var classe = _context.Class.Where(c => c.Id == classID).First();
            var students = getListStudentsWithGrade(classe, exam);
            ExamGradeStudent element = new ExamGradeStudent
            {
                Id = examID,
                name = exam.name,
                thema = exam.Thema,
                Examday = exam.Examday,
                Highscore = exam.Highscore,
                Students = students
            };
            return ApiResponses.GetResponse(_mapper.Map<ExamGradeStudentDTO>(element));
        }
        [HttpPost]
        public IActionResult SaveGrades(int classID, int examID, string grades)
        {
            List<Grade> gradeList = JsonConvert.DeserializeObject<List<Grade>>(grades);
            foreach(Grade g in gradeList)
            {
                var MYgrade = _context.Grade.Where(e => e.fk_Exam == examID).Where(e => e.fk_Student == g.fk_Student).FirstOrDefault();
                if (MYgrade == null)
                {
                    MYgrade = new Grade { fk_Exam = examID, fk_Student = g.fk_Student };
                    _context.Grade.Add(MYgrade);
                }
                MYgrade.Score = g.Score;
                MYgrade.grade = g.grade;
                _context.SaveChanges();
            }
            return Ok(new { success = true });
        }



        // GET: Exams
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Exam.Include(e => e.Modul);
            return ApiResponses.GetResponse(_mapper.Map<List<ExamAllDTO>>(applicationDbContext));
        }
        // GET: Exams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Exam == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }
            var exam = await _context.Exam
                .Include(e => e.Modul)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exam == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }
            return ApiResponses.GetResponse(exam);
        }
        // POST: Exams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Score,fk_Teacher,fk_Modul")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exam);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            ViewData["fk_Modul"] = new SelectList(_context.Modul, "Id", "Id", exam.fk_Modul);
            return ApiResponses.GetErrorResponse(2, "Not Valid");

        }
        // POST: Exams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Score,fk_Teacher,fk_Modul")] Exam exam)
        {
            if (id != exam.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.Id))
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
            ViewData["fk_Modul"] = new SelectList(_context.Modul, "Id", "Id", exam.fk_Modul);
            return ApiResponses.GetErrorResponse(2, "Not Valid");

        }
        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Exam == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Exam'  is null.");
            }
            var exam = await _context.Exam.FindAsync(id);
            if (exam != null)
            {
                _context.Exam.Remove(exam);
            }
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse(nameof(Index));
        }

    }
}
