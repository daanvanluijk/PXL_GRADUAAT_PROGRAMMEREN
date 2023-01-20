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
using Microsoft.AspNetCore.Identity;
using CSWeb1PE.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Query;

namespace CSWeb1PE.Controllers
{
    public class HandboekenController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public HandboekenController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager
        )
        {
            _dbContext = context;
            _userManager = userManager;
        }

        // GET: Handboeken
        [Authorize(Roles = "Admin,Lector,Student")]
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            Lector? lector = await _dbContext.Lectoren.Include(x => x.Gebruiker).FirstOrDefaultAsync(x => x.Gebruiker.Email == user.Email);
            Student? student = await _dbContext.Studenten.Include(x => x.Gebruiker).FirstOrDefaultAsync(x => x.Gebruiker.Email == user.Email);

            IQueryable<Handboek> handboeken = lector == null
                ? student == null
                    ? _dbContext.Handboeken
                    : _dbContext.Handboeken
                        .Where(h => _dbContext.Inschrijvingen
                            .Include(i => i.VakLector)
                            .Include(i => i.VakLector.Vak)
                            .Any(i => i.StudentId == student.StudentId && i.VakLector.Vak.HandboekId == h.HandboekId))
                : _dbContext.Handboeken
                    .Where(h => _dbContext.VakLectoren
                        .Include(vl => vl.Vak)
                        .Any(vl => vl.LectorId == lector.LectorId && vl.Vak.HandboekId == h.HandboekId));
            return View(await handboeken.ToListAsync());
        }

        // GET: Handboeken/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbContext.Handboeken == null)
            {
                return NotFound();
            }

            var handboek = await _dbContext.Handboeken
                .FirstOrDefaultAsync(m => m.HandboekId == id);
            if (handboek == null)
            {
                return NotFound();
            }

            return View(handboek);
        }

        // GET: Handboeken/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Handboeken/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HandboekId,Titel,Kostprijs,UitgifteDatum,Afbeelding")] Handboek handboek)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(handboek);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(handboek);
        }

        // GET: Handboeken/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dbContext.Handboeken == null)
            {
                return NotFound();
            }

            var handboek = await _dbContext.Handboeken.FindAsync(id);
            if (handboek == null)
            {
                return NotFound();
            }
            return View(handboek);
        }

        // POST: Handboeken/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("HandboekId,Titel,Kostprijs,UitgifteDatum,Afbeelding")] Handboek handboek)
        {
            if (id != handboek.HandboekId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(handboek);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HandboekExists(handboek.HandboekId))
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
            return View(handboek);
        }

        // GET: Handboeken/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbContext.Handboeken == null)
            {
                return NotFound();
            }

            var handboek = await _dbContext.Handboeken
                .FirstOrDefaultAsync(m => m.HandboekId == id);
            if (handboek == null)
            {
                return NotFound();
            }

            return View(handboek);
        }

        // POST: Handboeken/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbContext.Handboeken == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Handboeken'  is null.");
            }
            var handboek = await _dbContext.Handboeken.FindAsync(id);
            if (handboek != null)
            {
                _dbContext.Handboeken.Remove(handboek);
            }
            
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HandboekExists(int id)
        {
          return _dbContext.Handboeken.Any(e => e.HandboekId == id);
        }
    }
}
