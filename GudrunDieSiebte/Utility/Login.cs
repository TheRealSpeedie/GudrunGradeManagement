using GudrunDieSiebte.Controllers;
using GudrunDieSiebte.Data;
using GudrunDieSiebte.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;


namespace GudrunDieSiebte.Utility
{
    public class Login
    {
        public static Type GetUserRole(ClaimsPrincipal User, ApplicationDbContext _context)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var personId = _context.Person.Where(p => p.fk_user == userId).Select(p => p.Id).First();
                if (personId == null) return null;

                IQueryable<Student> students = _context.Student.Where(p => p.fk_Person == personId);
                IQueryable<Teacher> teachers = _context.Teacher.Where(p => p.fk_Person == personId);

                if (students.Count() > 0) return typeof(Student);

                if (teachers.Count() > 0) return typeof(Teacher);
                
            }
            return null;
          
        }
    }
}
