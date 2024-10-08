using GudrunDieSiebte.Data;
using GudrunDieSiebte.DTO;
using GudrunDieSiebte.Models;
using GudrunDieSiebte.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Web;
using static System.Net.WebRequestMethods;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace GudrunDieSiebte.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly BasicFunctions _basicFunctions;
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _logger = logger;
            _usermanager = userManager;
            _signInManager = signInManager; 
            _basicFunctions = new BasicFunctions(context);
        }
        public IActionResult Index()
        {
            _basicFunctions.SetConnection();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Version()
        {
            SqlConnection sqlConn = new SqlConnection(@"Server=[Server]; Database=Gudrun; User=sa; Password=[sa-passwort]");
            string query = "SELECT TOP 1 max(time), version FROM Version GROUP BY version Order by version DESC";
            SqlCommand command = new SqlCommand(query, sqlConn);
            SqlDataReader reader = null;
            command.Connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            string version = reader.GetString(1);
            command.Connection.Close();

            return ApiResponses.GetResponse(version);
        }
        public async Task<IActionResult> Login(string id)
        {
            var user = await _usermanager.FindByIdAsync(id);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return ApiResponses.GetResponse(new { LinkUsers = "https://localhost:7138/Home/Users", LinkGenerate = "https://localhost:7138/Students/Generate", user });
        }
        public async Task<IActionResult> Users()
        {
            var link = "https://localhost:7138/Home/Login/";
            var admins1 = await _usermanager.GetUsersInRoleAsync(Roles.getRoleString(Roles.RoleType.Admin));
            var admins = admins1.Select(x => new { Link = link + x.Id, x.UserName });
            var students1 = await _usermanager.GetUsersInRoleAsync(Roles.getRoleString(Roles.RoleType.Student));
            var students = students1.Select(x => new { Link = link + x.Id, x.UserName });
            var teachers1 = await _usermanager.GetUsersInRoleAsync(Roles.getRoleString(Roles.RoleType.Teacher));
            var teachers = teachers1.Select(x => new { Link = link + x.Id, x.UserName });
            Dictionary<string, IEnumerable> users = new Dictionary<string, IEnumerable>();
            users.Add("Admins", admins.OrderBy(u => u.UserName));
            users.Add("Students", students.OrderBy(u => u.UserName));
            users.Add("Teachers", teachers.OrderBy(u => u.UserName));
            return ApiResponses.GetResponse(new { Link = "https://localhost:7138/Students/Generate", users });
        }
    }
}