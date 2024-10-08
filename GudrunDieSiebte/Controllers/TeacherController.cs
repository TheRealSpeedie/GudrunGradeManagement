using GudrunDieSiebte.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GudrunDieSiebte.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        // GET: Teacher
        // oder
        // GET: Teacher/Index
        public ActionResult Index()
        {
            return View();
        }
        // GET: Teacher/Absence
        public ActionResult Absence()
        {
            return View();
        }
        // GET: Teacher/Grade
        public ActionResult Grade()
        {
            return View();
        }
        // GET: Teacher/Module
        public ActionResult Module()
        {
            return View();
        }
        // GET: Teacher/EditProfile
        public ActionResult EditProfile()
        {
            return View();
        }
        public ActionResult AddTest()
        {
            return View();
        }
		public ActionResult AddGrade()
		{
			return View();
		}
		// GET: Teacher/TeachingCompany
		public ActionResult TeachingCompany()
        {
            return View();
        }
    }
}
