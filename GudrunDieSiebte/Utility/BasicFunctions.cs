using GudrunDieSiebte.Data;
using GudrunDieSiebte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace GudrunDieSiebte.Utility
{
    public class BasicFunctions
    {
        private readonly ApplicationDbContext _context;

        public BasicFunctions(ApplicationDbContext context)
        {
            _context = context;
        }
        public Student getStudent(ClaimsPrincipal User, String includeObjekt = "")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Person.Where(p => p.fk_user == userId).First();
            var person = _context.Person.Where(p => p.fk_user == userId).First();
            switch (includeObjekt)
            {
                case "Class":
                    var studentWithClass = _context.Student.Where(s => s.fk_Person == person.Id).Include(s => s.Class).First();
                    return studentWithClass;
                default:
                    var student = _context.Student.Where(s => s.fk_Person == person.Id).First();
                    return student;
            }   
        }
        public Teacher getTeacher(ClaimsPrincipal User)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var person = _context.Person.Where(p => p.fk_user == userId).First();
            var teacher = _context.Teacher.Where(t => t.fk_Person == person.Id).First();
            return teacher;
        }
        public Person getPerson(ClaimsPrincipal User)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var person = _context.Person.Where(p => p.fk_user == userId).Include(p => p.Appointments).First();
            return person;
        }
        public void AddToListIfNotInList<T>(List<T> list, T obj)
        {
            if (!list.Contains(obj))
            {
                list.Add(obj);
            }
        }
        public Student getStudentWithClass(ClaimsPrincipal User)
        {
            var person = getPerson(User);
            var student = _context.Student.Where(s => s.fk_Person == person.Id).Include(s => s.Class).First();
            return student;
        }
        public List<T> checkForDuplicate<T>(List<T> mylist)
        {
            List<T> list = new List<T>();
            foreach(var e in mylist)
            {
                AddToListIfNotInList(list, e);
            }
            return list;
        }
        private  bool TestConnectionString(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public  void SetConnection()
        {
            var apisettingsFile = "appsettings.json";
            try
            { 
                var jsonContent = File.ReadAllText(apisettingsFile);
                var jsonObject = JObject.Parse(jsonContent);
                var connectionString1 = "Server=WSLEHRLING1\\SQL2019Dev;Database=Gudrun;User Id=sa;Password=tplus_sa01;";
                var connectionString2 = "Server=DESKTOP-K88GTOC\\SQL_2019;Database=Gudrun;User Id=sa;Password=tplus_sa01;";
                if (TestConnectionString(connectionString1))
                {
                    jsonObject["ConnectionStrings"]["DefaultConnection"] = connectionString1;
                }
                else if (TestConnectionString(connectionString2))
                {
                    jsonObject["ConnectionStrings"]["DefaultConnection"] = connectionString2;
                }
                else
                {
                    throw new Exception("Keiner der Connection Strings funktioniert.");
                }
                File.WriteAllText(apisettingsFile, jsonObject.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ein Fehler ist aufgetreten: " + ex.Message);
            }
        }

    }
}
