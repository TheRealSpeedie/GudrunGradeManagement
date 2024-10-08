using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GudrunDieSiebte.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        // oder
        // GET: Contact/Index
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangeRequest()
        {
            return View();
        }
    }
}
