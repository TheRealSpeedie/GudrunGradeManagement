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
using AutoMapper;
using GudrunDieSiebte.DTO;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace GudrunDieSiebte.Controllers
{
    public class TeachingCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TeachingCompaniesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: TeachingCompanies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.TeachingCompany.Include(t => t.Students).ThenInclude(s => s.Person).ToListAsync();

            return ApiResponses.GetResponse(_mapper.Map<List<TeachingCompanyAllDTO>>(applicationDbContext));
        }
        // GET: TeachingCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TeachingCompany == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");

            }

            var teachingCompany = await _context.TeachingCompany
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teachingCompany == null)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");

            }

            return ApiResponses.GetResponse(teachingCompany);
        }
        // POST: TeachingCompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string address, string city)
        {
            TeachingCompany teachingCompany = new TeachingCompany
            {
                Name = name,
                Address = address,
                City = city
            };
            if (teachingCompany != null && name != null && address != null && city != null)
            {
                string allletters = name + address + city;
                string allowableLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890äöüÄÖÜàéèÀÉÈ ";
                foreach (char c in allletters)
                {
                    if (!allowableLetters.Contains(c.ToString()))
                    {
                        return ApiResponses.GetErrorResponse(0, "Ungültiges Zeichen gefunden");
                    }
                }
                _context.Add(teachingCompany);
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(nameof(Index));
            }
            return ApiResponses.GetErrorResponse(0, teachingCompany.ToString());

        }
        // POST: TeachingCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name, string address, string city)
        {
            TeachingCompany teachingCompany = await _context.TeachingCompany.FirstOrDefaultAsync(m => m.Id == id);
            if (id != teachingCompany.Id)
            {
                return ApiResponses.GetErrorResponse(1, "Not Found");
            }
            if (name != null && address != null && city != null)
            {
                string allletters = name + address + city;
                string allowableLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890äöüÄÖÜàéèÀÉÈ ";
                foreach (char c in allletters)
                {
                    if (!allowableLetters.Contains(c.ToString()))
                    {
                        return ApiResponses.GetErrorResponse(0, "Ungültiges Zeichen gefunden");
                    }
                }
                teachingCompany.Name = name;
                teachingCompany.Address = address;
                teachingCompany.City = city;
                await _context.SaveChangesAsync();
                return ApiResponses.GetResponse(_mapper.Map<TeachingCompanyAllDTO>(teachingCompany));
            }
            return ApiResponses.GetErrorResponse(2, "Not Valid");

        }
        // POST: TeachingCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeachingCompany == null)
            {
                return ApiResponses.GetErrorResponse(0, "Entity set 'TeachingCompany'  is null.");
            }
            var teachingCompany = await _context.TeachingCompany.FindAsync(id);
            if (teachingCompany != null)
            {
                _context.TeachingCompany.Remove(teachingCompany);
            }
            
            await _context.SaveChangesAsync();
            return ApiResponses.GetResponse("Delete success");
        }
        private bool TeachingCompanyExists(int id)
        {
          return _context.TeachingCompany.Any(e => e.Id == id);
        }
       
    }
}
