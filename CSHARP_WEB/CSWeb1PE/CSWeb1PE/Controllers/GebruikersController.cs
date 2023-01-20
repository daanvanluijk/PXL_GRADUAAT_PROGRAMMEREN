using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSWeb1PE.Data;
using CSWeb1PE.Models;
using CSWeb1PE.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CSWeb1PE.Controllers
{
    public class GebruikersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GebruikersController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _dbContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Gebruikers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<GebruikerViewModel> gebruikers = new List<GebruikerViewModel>();
            foreach (Gebruiker gebruiker in _dbContext.Gebruikers)
            {
                IdentityUser identityUser = await _userManager.FindByEmailAsync(gebruiker.Email);
                string role = "Geen";
                if (identityUser != null)
                {
                    IList<string> roles = await _userManager.GetRolesAsync(identityUser);
                    if (roles.Count > 0)
                    {
                        role = roles.First();
                    }
                }

                gebruikers.Add(new GebruikerViewModel()
                {
                    Gebruiker = gebruiker,
                    Role = role,
                });
            }
            return View(gebruikers);
        }

        // GET: Gebruikers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbContext.Gebruikers == null)
            {
                return NotFound();
            }

            Gebruiker? gebruiker = await _dbContext.Gebruikers
                .FirstOrDefaultAsync(m => m.GebruikerId == id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            string role = (await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(gebruiker.Email)))[0];

            return View(new GebruikerViewModel
            {
                Gebruiker = gebruiker,
                Role = role,
            });
        }

        // GET: Gebruikers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(_dbContext.Roles, "Name", "Name");
            return View();
        }

        // POST: Gebruikers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GebruikerViewModel gebruikerViewModel)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(gebruikerViewModel.Gebruiker);

                // Add new IdentityUser
                IdentityUser user = new IdentityUser()
                {
                    UserName = gebruikerViewModel.Gebruiker.Email,
                    Email = gebruikerViewModel.Gebruiker.Email,
                };
                IdentityResult result = await _userManager.CreateAsync(user, gebruikerViewModel.Paswoord);
                if (!result.Succeeded)
                {
                    Problem(result.Errors.First().Code);
                }
                result = await _userManager.AddToRoleAsync(user, gebruikerViewModel.Role);
                if (!result.Succeeded)
                {
                    Problem(result.Errors.First().Code);
                }

                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Roles"] = new SelectList(_dbContext.Roles, "Name", "Name");
            return View(gebruikerViewModel);
        }

        // GET: Gebruikers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dbContext.Gebruikers == null)
            {
                return NotFound();
            }

            Gebruiker? gebruiker = await _dbContext.Gebruikers.FindAsync(id);
            if (gebruiker == null)
            {
                return NotFound();
            }
            IdentityUser identityUser = await _userManager.FindByEmailAsync(gebruiker.Email);
            string role = "Geen";
            if (identityUser != null)
            {
                IList<string> roles = await _userManager.GetRolesAsync(identityUser);
                if (roles.Count > 0)
                {
                    role = roles.First();
                }
            }

            GebruikerViewModel gebruikerViewModel = new GebruikerViewModel()
            {
                Gebruiker = gebruiker,
                Role = role,
            };

            ViewData["Roles"] = new SelectList(_dbContext.Roles, "Name", "Name");
            return View(gebruikerViewModel);
        }

        // POST: Gebruikers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GebruikerViewModel gebruikerViewModel)
        {
            if (id != gebruikerViewModel.Gebruiker.GebruikerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(gebruikerViewModel.Gebruiker);
                    await ChangeGebruikerType(gebruikerViewModel.Gebruiker, gebruikerViewModel.Role);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GebruikerExists(gebruikerViewModel.Gebruiker.GebruikerId))
                    {
                        ViewData["Roles"] = new SelectList(_dbContext.Roles, "Name", "Name");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Roles"] = new SelectList(_dbContext.Roles, "Name", "Name");
            return View(gebruikerViewModel);
        }

        // GET: Gebruikers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbContext.Gebruikers == null)
            {
                return NotFound();
            }

            var gebruiker = await _dbContext.Gebruikers
                .FirstOrDefaultAsync(m => m.GebruikerId == id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            return View(gebruiker);
        }

        // POST: Gebruikers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbContext.Gebruikers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Gebruikers' is null.");
            }

            Gebruiker? gebruiker = await _dbContext.Gebruikers.FindAsync(id);
            if (gebruiker != null)
            {
                _dbContext.Gebruikers.Remove(gebruiker);
                Student? student = await _dbContext.Studenten.FirstOrDefaultAsync(x => x.GebruikerId == gebruiker.GebruikerId);
                if (student != null)
                    _dbContext.Studenten.Remove(student);
                Lector? lector = await _dbContext.Lectoren.FirstOrDefaultAsync(x => x.GebruikerId == gebruiker.GebruikerId);
                if (lector != null)
                    _dbContext.Lectoren.Remove(lector);

                IdentityUser? user = await _userManager.FindByEmailAsync(gebruiker.Email);
                if (user != null)
                {

                    await _userManager.DeleteAsync(user);
                }
            }
            
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GebruikerExists(int id)
        {
          return _dbContext.Gebruikers.Any(e => e.GebruikerId == id);
        }

        private async Task ChangeGebruikerType(Gebruiker gebruiker, string newRole)
        {
            // Identity stuff
            IdentityUser user = await _userManager.FindByEmailAsync(gebruiker.Email);
            string? currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (currentRole != null)
            {
                await _userManager.RemoveFromRoleAsync(user, currentRole);
            }
            await _userManager.AddToRoleAsync(user, newRole);

            // Student or Lector?
            if (newRole == "Lector")
            {
                Lector? existingLector = await _dbContext.Lectoren.FirstOrDefaultAsync(x => x.GebruikerId == gebruiker.GebruikerId);
                if (existingLector == null)
                {
                    Lector lector = new Lector()
                    {
                        Gebruiker = gebruiker,
                        GebruikerId = gebruiker.GebruikerId,
                    };
                    _dbContext.Add(lector);
                    await _dbContext.SaveChangesAsync();
                }
            }
            else if (newRole == "Student")
            {
                Student? existingStudent = await _dbContext.Studenten.FirstOrDefaultAsync(x => x.GebruikerId == gebruiker.GebruikerId);
                if (existingStudent == null)
                {
                    Student student = new Student()
                    {
                        Gebruiker = gebruiker,
                        GebruikerId = gebruiker.GebruikerId,
                    };
                    _dbContext.Add(student);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
