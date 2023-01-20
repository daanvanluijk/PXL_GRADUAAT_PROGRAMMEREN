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
using CSWeb1PE.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Identity;

namespace CSWeb1PE.Controllers
{
    public class VakkenController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public VakkenController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager
        )
        {
            _dbContext = context;
            _userManager = userManager;
        }

        // GET: Vakken
        [Authorize(Roles = "Admin,Lector,Student")]
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            Lector? lector = await _dbContext.Lectoren.Include(x => x.Gebruiker).FirstOrDefaultAsync(x => x.Gebruiker.Email == user.Email);
            Student? student = await _dbContext.Studenten.Include(x => x.Gebruiker).FirstOrDefaultAsync(x => x.Gebruiker.Email == user.Email);

            IIncludableQueryable<Vak, Handboek> vakken = lector == null
                ? student == null
                    ? _dbContext.Vakken.Include(v => v.Handboek)
                    : _dbContext.Vakken
                        .Where(v => _dbContext.Inschrijvingen.Any(vl => vl.StudentId == student.StudentId && vl.VakLector.VakId == v.VakId))
                        .Include(v => v.Handboek)
                : _dbContext.Vakken
                    .Where(v => _dbContext.VakLectoren.Any(vl => vl.VakId == v.VakId && vl.LectorId == lector.LectorId))
                    .Include(v => v.Handboek);
            return View(await vakken.Select(x => new VakViewModel()
            {
                VakId = x.VakId,
                VakNaam = x.VakNaam,
                Studiepunten = x.Studiepunten,
                Handboek = x.Handboek.Titel,
            }).ToListAsync());
        }

        // GET: Vakken/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbContext.Vakken == null)
            {
                return NotFound();
            }

            VakLector? vakLector = await _dbContext.VakLectoren
                .Include(x => x.Lector.Gebruiker)
                .Include(x => x.Vak.Handboek)
                .FirstOrDefaultAsync(m => m.VakId == id);
            if (vakLector == null)
            {
                return NotFound();
            }

            return View(new VakViewModel()
            {
                VakId = vakLector.VakId,
                VakNaam = vakLector.Vak.VakNaam,
                Studiepunten = vakLector.Vak.Studiepunten,
                Handboek = vakLector.Vak.Handboek.Titel,
                Lector = vakLector.Lector.Gebruiker.Naam + " " + vakLector.Lector.Gebruiker.Voornaam,
            });
        }

        // GET: Vakken/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            IEnumerable<string> lectorNamen = _dbContext.Lectoren
                .Include(x => x.Gebruiker)
                .ToList()
                .Where(x => _userManager.IsInRoleAsync(_userManager.FindByEmailAsync(x.Gebruiker.Email).Result, "Lector").Result)
                .Select(x => x.Gebruiker.Naam + " " + x.Gebruiker.Voornaam);
            ViewData["Handboeken"] = new SelectList(_dbContext.Handboeken, "Titel", "Titel");
            ViewData["Lectoren"] = new SelectList(lectorNamen);
            return View();
        }

        // POST: Vakken/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VakViewModel vakViewModel)
        {
            Handboek? handboek = await _dbContext.Handboeken.FirstOrDefaultAsync(x => x.Titel == vakViewModel.Handboek);
            if (handboek == null)
            {
                return NotFound();
            }

            Vak vak = new Vak()
            {
                VakNaam = vakViewModel.VakNaam,
                Studiepunten = vakViewModel.Studiepunten,
                HandboekId = handboek.HandboekId,
                Handboek = handboek,
            };

            Lector? lector = await _dbContext.Lectoren.FirstOrDefaultAsync(x => x.Gebruiker.Naam + " " + x.Gebruiker.Voornaam == vakViewModel.Lector);
            if (lector == null)
            {
                return NotFound();
            }

            VakLector vakLector = new VakLector()
            {
                Vak = vak,
                Lector = lector,
                LectorId = lector.LectorId,
            };

            if (ModelState.IsValid)
            {
                _dbContext.Add(vak);
                await _dbContext.SaveChangesAsync();
                vakLector.VakId = _dbContext.Vakken.Max(x => x.VakId);
                _dbContext.Add(vakLector);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<string> lectorNamen = _dbContext.Lectoren
                .Include(x => x.Gebruiker)
                .ToList()
                .Where(x => _userManager.IsInRoleAsync(_userManager.FindByEmailAsync(x.Gebruiker.Email).Result, "Lector").Result)
                .Select(x => x.Gebruiker.Naam + " " + x.Gebruiker.Voornaam);
            ViewData["Handboeken"] = new SelectList(_dbContext.Handboeken, "Titel", "Titel", vakViewModel.Handboek);
            ViewData["Lectoren"] = new SelectList(lectorNamen, vakViewModel.Lector);
            return View(vakViewModel);
        }

        // GET: Vakken/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dbContext.Vakken == null)
            {
                return NotFound();
            }

            VakLector? vakLector = await _dbContext.VakLectoren
                .Include(x => x.Lector.Gebruiker)
                .Include(x => x.Vak.Handboek)
                .FirstOrDefaultAsync(x => x.VakId == id);
            if (vakLector == null)
            {
                return NotFound();
            }

            VakViewModel vakViewModel = new VakViewModel()
            {
                VakId = id.Value,
                VakNaam = vakLector.Vak.VakNaam,
                Studiepunten = vakLector.Vak.Studiepunten,
                Handboek = vakLector.Vak.Handboek.Titel,
                Lector = vakLector.Lector.Gebruiker.Naam + " " + vakLector.Lector.Gebruiker.Voornaam,
            };

            IEnumerable<string> lectorNamen = _dbContext.Lectoren
                .Include(x => x.Gebruiker)
                .ToList()
                .Where(x => _userManager.IsInRoleAsync(_userManager.FindByEmailAsync(x.Gebruiker.Email).Result, "Lector").Result)
                .Select(x => x.Gebruiker.Naam + " " + x.Gebruiker.Voornaam);
            ViewData["Handboeken"] = new SelectList(_dbContext.Handboeken, "Titel", "Titel", vakLector.Vak.Handboek.Titel);
            ViewData["Lectoren"] = new SelectList(lectorNamen, vakLector.Lector.Gebruiker.Naam + " " + vakLector.Lector.Gebruiker.Voornaam);
            return View(vakViewModel);
        }

        // POST: Vakken/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, VakViewModel vakViewModel)
        {
            VakLector? vakLector = await _dbContext.VakLectoren
                .Include(x => x.Lector.Gebruiker)
                .Include(x => x.Vak.Handboek)
                .FirstOrDefaultAsync(x => x.VakId == id);
            if (vakLector == null)
            {
                return NotFound();
            }
            vakLector.Vak.VakNaam = vakViewModel.VakNaam;
            vakLector.Vak.Studiepunten = vakViewModel.Studiepunten;

            Handboek? handboek = await _dbContext.Handboeken.FirstOrDefaultAsync(x => x.Titel == vakViewModel.Handboek);
            if (handboek == null)
            {
                return NotFound();
            }
            vakLector.Vak.Handboek = handboek;
            vakLector.Vak.HandboekId = handboek.HandboekId;

            Lector? lector = await _dbContext.Lectoren.FirstOrDefaultAsync(x => x.Gebruiker.Naam + " " + x.Gebruiker.Voornaam == vakViewModel.Lector);
            if (lector == null)
            {
                return NotFound();
            }
            vakLector.Lector = lector;
            vakLector.LectorId = lector.LectorId;

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(vakLector);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VakExists(id))
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

            IEnumerable<string> lectorNamen = _dbContext.Lectoren
                .Include(x => x.Gebruiker)
                .ToList()
                .Where(x => _userManager.IsInRoleAsync(_userManager.FindByEmailAsync(x.Gebruiker.Email).Result, "Lector").Result)
                .Select(x => x.Gebruiker.Naam + " " + x.Gebruiker.Voornaam);
            ViewData["Handboeken"] = new SelectList(_dbContext.Handboeken, "Titel", "Titel", handboek.Titel);
            ViewData["Lectoren"] = new SelectList(_dbContext.Lectoren, vakViewModel.Lector);
            return View(vakViewModel);
        }

        // GET: Vakken/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbContext.Vakken == null)
            {
                return NotFound();
            }

            VakLector? vakLector = await _dbContext.VakLectoren
                .Include(x => x.Vak.Handboek)
                .Include(x => x.Lector.Gebruiker)
                .FirstOrDefaultAsync(x => x.VakId == id);
            if (vakLector == null)
            {
                return NotFound();
            }

            return View(new VakViewModel()
            {
                VakId = vakLector.VakId,
                VakNaam = vakLector.Vak.VakNaam,
                Studiepunten = vakLector.Vak.Studiepunten,
                Handboek = vakLector.Vak.Handboek.Titel,
                Lector = vakLector.Lector.Gebruiker.Naam + " " + vakLector.Lector.Gebruiker.Voornaam,
            });
        }

        // POST: Vakken/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbContext.Vakken == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vakken' is null.");
            }

            Vak? vak = await _dbContext.Vakken.FindAsync(id);
            if (vak != null)
            {
                _dbContext.Vakken.Remove(vak);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VakExists(int id)
        {
            return _dbContext.Vakken.Any(e => e.VakId == id);
        }
    }
}
