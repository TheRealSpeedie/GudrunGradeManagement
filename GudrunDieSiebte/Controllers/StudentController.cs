using GudrunDieSiebte.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GudrunDieSiebte.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        // oder
        // GET: Student/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: Student/Classes
        public ActionResult Classes()
        {
            return View();
        }
        // GET: Student/Module
        public ActionResult Module()
        {
            return View();
        }
        // GET: Student/Teacherlist
        public ActionResult Teacherlist()
        {
            return View();
        }
        // GET: Student/Timetable
        public ActionResult Timetable()
        {
            return View();
        }
        public ActionResult ToDo()
        {
            return View();
        }
    }
}
