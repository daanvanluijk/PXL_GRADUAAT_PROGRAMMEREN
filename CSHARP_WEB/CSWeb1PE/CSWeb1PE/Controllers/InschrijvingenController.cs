using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSWeb1PE.Data;
using CSWeb1PE.Models;
using Microsoft.EntityFrameworkCore.Query;
using CSWeb1PE.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace CSWeb1PE.Controllers
{
    public class InschrijvingenController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public InschrijvingenController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager
        )
        {
            _dbContext = context;
            _userManager = userManager;
        }

        // GET: Inschrijvingen
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            IQueryable<Inschrijving> inschrijvingen = _dbContext.Inschrijvingen
                .Include(i => i.AcademieJaar)
                .Include(i => i.Student)
                .Include(i => i.Student.Gebruiker)
                .Include(i => i.VakLector)
                .Include(i => i.VakLector.Vak);
            return View(inschrijvingen
                .ToList()
                .Where(x => _userManager.IsInRoleAsync(_userManager.FindByEmailAsync(x.Student.Gebruiker.Email).Result, "Student").Result)
                .Select(x => new InschrijvingViewModel()
            {
                InschrijvingId = x.InschrijvingId,
                Vak = x.VakLector.Vak.VakNaam,
                Student = GetGebruikerAsString(x.Student.Gebruiker),
                AcademieJaar = GetAcademieJaarAsString(x.AcademieJaar),
            }).ToList());
        }

        // GET: Inschrijvingen/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbContext.Inschrijvingen == null)
            {
                return NotFound();
            }

            Inschrijving? inschrijving = await _dbContext.Inschrijvingen
                .Include(i => i.AcademieJaar)
                .Include(i => i.Student)
                .Include(i => i.Student.Gebruiker)
                .Include(i => i.VakLector)
                .Include(i => i.VakLector.Vak)
                .FirstOrDefaultAsync(m => m.InschrijvingId == id);
            if (inschrijving == null)
            {
                return NotFound();
            }

            return View(new InschrijvingViewModel()
            {
                InschrijvingId = inschrijving.InschrijvingId,
                Vak = inschrijving.VakLector.Vak.VakNaam,
                Student = GetGebruikerAsString(inschrijving.Student.Gebruiker),
                AcademieJaar = GetAcademieJaarAsString(inschrijving.AcademieJaar),
            });
        }

        // GET: Inschrijvingen/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["AcademieJaren"] = new SelectList(_dbContext.AcademieJaren.Select(x => GetAcademieJaarAsString(x)));
            List<Student> studenten = new List<Student>();
            foreach (Student student in _dbContext.Studenten.Include(x => x.Gebruiker))
            {
                IdentityUser user = await _userManager.FindByEmailAsync(student.Gebruiker.Email);
                if (await _userManager.IsInRoleAsync(user, "Student"))
                {
                    studenten.Add(student);
                }
            }
            ViewData["Studenten"] = new SelectList(studenten.Select(x => GetGebruikerAsString(x.Gebruiker)));
            ViewData["Vakken"] = new SelectList(_dbContext.VakLectoren.Select(x => x.Vak.VakNaam));
            return View();
        }

        // POST: Inschrijvingen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(InschrijvingViewModel inschrijvingViewModel)
        {
            VakLector? vakLector = await _dbContext.VakLectoren
                .Include(vl => vl.Vak)
                .Include(vl => vl.Lector)
                .FirstOrDefaultAsync(vl => vl.Vak.VakNaam == inschrijvingViewModel.Vak);
            if (vakLector == null)
            {
                return NotFound();
            }

            Student? student = null;
            foreach (Student student1 in _dbContext.Studenten.Include(x => x.Gebruiker))
            {
                if (GetGebruikerAsString(student1.Gebruiker) == inschrijvingViewModel.Student)
                {
                    student = student1;
                }
            }
            if (student == null)
            {
                return NotFound();
            }

            AcademieJaar? academieJaar = null;
            foreach (AcademieJaar academieJaar1 in _dbContext.AcademieJaren)
            {
                if (GetAcademieJaarAsString(academieJaar1) == inschrijvingViewModel.AcademieJaar)
                {
                    academieJaar = academieJaar1;
                }
            }
            if (academieJaar == null)
            {
                return NotFound();
            }

            Inschrijving inschrijving = new Inschrijving()
            {
                VakLectorId = vakLector.VakLectorId,
                VakLector = vakLector,
                StudentId = student.StudentId,
                Student = student,
                AcademieJaarId = academieJaar.AcademieJaarId,
                AcademieJaar = academieJaar,
            };

            if (ModelState.IsValid)
            {
                _dbContext.Add(inschrijving);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AcademieJaren"] = new SelectList(_dbContext.AcademieJaren.Select(x => GetAcademieJaarAsString(x)), inschrijvingViewModel.AcademieJaar);
            ViewData["Studenten"] = new SelectList(_dbContext.Studenten.Select(x => GetGebruikerAsString(x.Gebruiker)), inschrijvingViewModel.Student);
            ViewData["Vakken"] = new SelectList(_dbContext.VakLectoren.Select(x => x.Vak.VakNaam), inschrijvingViewModel.Vak);
            return View(inschrijvingViewModel);
        }

        // GET: Inschrijvingen/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dbContext.Inschrijvingen == null)
            {
                return NotFound();
            }

            Inschrijving? inschrijving = await _dbContext.Inschrijvingen
                .Include(i => i.AcademieJaar)
                .Include(i => i.Student)
                .Include(i => i.Student.Gebruiker)
                .Include(i => i.VakLector)
                .Include(i => i.VakLector.Vak)
                .FirstOrDefaultAsync(x => x.InschrijvingId == id);
            if (inschrijving == null)
            {
                return NotFound();
            }

            InschrijvingViewModel inschrijvingViewModel = new InschrijvingViewModel()
            {
                InschrijvingId = inschrijving.InschrijvingId,
                Vak = inschrijving.VakLector.Vak.VakNaam,
                Student = GetGebruikerAsString(inschrijving.Student.Gebruiker),
                AcademieJaar = GetAcademieJaarAsString(inschrijving.AcademieJaar),
            };

            ViewData["AcademieJaren"] = new SelectList(_dbContext.AcademieJaren.Select(x => GetAcademieJaarAsString(x)), inschrijvingViewModel.AcademieJaar);
            ViewData["Studenten"] = new SelectList(_dbContext.Studenten.Select(x => GetGebruikerAsString(x.Gebruiker)), inschrijvingViewModel.Student);
            ViewData["Vakken"] = new SelectList(_dbContext.VakLectoren.Select(x => x.Vak.VakNaam), inschrijvingViewModel.Vak);
            return View(inschrijvingViewModel);
        }

        // POST: Inschrijvingen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, InschrijvingViewModel inschrijvingViewModel)
        {
            Inschrijving? inschrijving = await _dbContext.Inschrijvingen.FindAsync(id);
            if (inschrijving == null)
            {
                return NotFound();
            }

            Student? student = null;
            foreach (Student student1 in _dbContext.Studenten.Include(x => x.Gebruiker))
            {
                if (GetGebruikerAsString(student1.Gebruiker) == inschrijvingViewModel.Student)
                {
                    student = student1;
                }
            }
            if (student == null)
            {
                return NotFound();
            }
            inschrijving.StudentId = student.StudentId;
            inschrijving.Student = student;

            VakLector? vakLector = await _dbContext.VakLectoren.Include(x => x.Vak).FirstOrDefaultAsync(x => x.Vak.VakNaam == inschrijvingViewModel.Vak);
            if (vakLector == null)
            {
                return NotFound();
            }
            inschrijving.VakLectorId = vakLector.VakLectorId;
            inschrijving.VakLector = vakLector;

            AcademieJaar? academieJaar = null;
            foreach (AcademieJaar academieJaar1 in _dbContext.AcademieJaren)
            {
                if (GetAcademieJaarAsString(academieJaar1) == inschrijvingViewModel.AcademieJaar)
                {
                    academieJaar = academieJaar1;
                }
            }
            if (academieJaar == null)
            {
                return NotFound();
            }
            inschrijving.AcademieJaarId = academieJaar.AcademieJaarId;
            inschrijving.AcademieJaar = academieJaar;

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(inschrijving);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InschrijvingExists(inschrijving.InschrijvingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AcademieJaren"] = new SelectList(_dbContext.AcademieJaren.Select(x => GetAcademieJaarAsString(x)), inschrijvingViewModel.AcademieJaar);
            ViewData["Studenten"] = new SelectList(_dbContext.Studenten.Select(x => GetGebruikerAsString(x.Gebruiker)), inschrijvingViewModel.Student);
            ViewData["Vakken"] = new SelectList(_dbContext.VakLectoren.Select(x => x.Vak.VakNaam), inschrijvingViewModel.Vak);
            return View(inschrijvingViewModel);
        }

        // GET: Inschrijvingen/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbContext.Inschrijvingen == null)
            {
                return NotFound();
            }

            Inschrijving? inschrijving = await _dbContext.Inschrijvingen
                .Include(i => i.AcademieJaar)
                .Include(i => i.Student)
                .Include(i => i.Student.Gebruiker)
                .Include(i => i.VakLector)
                .Include(i => i.VakLector.Vak)
                .FirstOrDefaultAsync(m => m.InschrijvingId == id);
            if (inschrijving == null)
            {
                return NotFound();
            }

            return View(new InschrijvingViewModel()
            {
                InschrijvingId = inschrijving.InschrijvingId,
                Vak = inschrijving.VakLector.Vak.VakNaam,
                Student = GetGebruikerAsString(inschrijving.Student.Gebruiker),
                AcademieJaar = GetAcademieJaarAsString(inschrijving.AcademieJaar),
            });
        }

        // POST: Inschrijvingen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbContext.Inschrijvingen == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Inschrijvingen'  is null.");
            }
            Inschrijving? inschrijving = await _dbContext.Inschrijvingen.FindAsync(id);
            if (inschrijving != null)
            {
                _dbContext.Inschrijvingen.Remove(inschrijving);
            }
            
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InschrijvingExists(int id)
        {
          return _dbContext.Inschrijvingen.Any(e => e.InschrijvingId == id);
        }

        private static string GetAcademieJaarAsString(AcademieJaar academieJaar)
        {
            return academieJaar.StartDatum.Year + " - " + (academieJaar.StartDatum.Year + 1);
        }

        private static string GetGebruikerAsString(Gebruiker gebruiker)
        {
            return gebruiker.Naam + " " + gebruiker.Voornaam;
        }
    }
}
