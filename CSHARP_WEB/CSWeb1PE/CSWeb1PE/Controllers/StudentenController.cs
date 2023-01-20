using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSWeb1PE.Data;
using CSWeb1PE.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Identity;
using CSWeb1PE.Models.ViewModels;

namespace CSWeb1PE.Controllers
{
    public class StudentenController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentenController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager
        )
        {
            _dbContext = context;
            _userManager = userManager;
        }

        // GET: Studenten
        [Authorize(Roles = "Admin,Lector")]
        public async Task<IActionResult> Index(string search)
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            Lector? lector = await _dbContext.Lectoren.Include(x => x.Gebruiker).FirstOrDefaultAsync(x => x.Gebruiker.Email == user.Email);

            IEnumerable<Student> studenten =
                await _userManager.IsInRoleAsync(user, "Lector")
                ? _dbContext.Studenten
                    .Include(s => s.Gebruiker)
                    .Where(s => _dbContext.Inschrijvingen.Any(i => i.StudentId == s.StudentId && i.VakLector.LectorId == lector.LectorId))
                    .ToList()
                    .Where(x => _userManager.IsInRoleAsync(_userManager.FindByEmailAsync(x.Gebruiker.Email).Result, "Student").Result)
                : _dbContext.Studenten.Include(s => s.Gebruiker)
                    .ToList()
                    .Where(x => _userManager.IsInRoleAsync(_userManager.FindByEmailAsync(x.Gebruiker.Email).Result, "Student").Result);
            return View(studenten.ToList());
        }

        // GET: Studenten/Details/5
        [Authorize(Roles = "Admin,Lector")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbContext.Studenten == null)
            {
                return NotFound();
            }

            Student? student = await _dbContext.Studenten
                .Include(x => x.Gebruiker)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            List<Vak> vakken = await _dbContext.Vakken
                .Where(v => _dbContext.Inschrijvingen
                    .Include(x => x.VakLector)
                    .Include(x => x.VakLector.Vak)
                    .Include(x => x.VakLector.Vak.Handboek)
                    .Any(x => x.StudentId == student.StudentId && x.VakLector.VakId == v.VakId))
                .Include(v => v.Handboek)
                .ToListAsync();

            List<Handboek> handboeken = vakken.Select(x => x.Handboek).ToList();

            StudentViewModel studentViewModel = new StudentViewModel()
            {
                Student = student,
                Vakken = vakken,
                Handboeken = handboeken,
            };

            return View(studentViewModel);
        }
    }
}
