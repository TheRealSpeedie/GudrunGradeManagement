using Microsoft.AspNetCore.Mvc;

namespace GudrunDieSiebte.Controllers
{
    public class GeneralController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        // GET: Student/Modullist
        public ActionResult Modullist()
        {
            return View();
        }
        // GET: Student/Studentlist
        public ActionResult Studentlist()
        {
            return View();
        }
        // GET: Student/Teacherlist
        public ActionResult Teacherlist()
        {
            return View();
        }
        // GET: Student/TeachingCompanylist
        public ActionResult TeachingCompanylist()
        {
            return View();
        }
    }
}
