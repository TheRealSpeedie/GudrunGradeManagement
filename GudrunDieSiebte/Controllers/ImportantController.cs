using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GudrunDieSiebte.Controllers
{
    public class ImportantController : Controller
    {
        // GET: Important
        // oder
        // GET: Important/Index
        public ActionResult Index()
        {
            return View();
        }
        // GET: Important
        // oder
        // GET: Important/Appointments
        public ActionResult Appointments()
        {
            return View();
        }
        // GET: Important
        // oder
        // GET: Important/Course
        public ActionResult Course()
        {
            return View();
        }
        // GET: Important
        // oder
        // GET: Important/ExamTable
        public ActionResult ExamTable()
        {
            return View();
        }
        // GET: Important
        // oder
        // GET: Important/Grades
        public ActionResult Grades()
        {
            return View();
        }
        // GET: Important
        // oder
        // GET: Important/SemesterSchedule
        public ActionResult SemesterSchedule()
        {
            return View();
        }
        // GET: Important
        // oder
        // GET: Important/Testimony
        public ActionResult Testimony()
        {
            return View();
        }
    }
}
