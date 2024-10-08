using GudrunDieSiebte.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GudrunDieSiebte.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Student()
        {
            return View();
        }
        public ActionResult Teacher()
        {
            return View();
        }
        public ActionResult TeachingCompany()
        {
            return View();
        }
        public ActionResult SchoolManagement()
        {
            return View();
        }
    }
}
