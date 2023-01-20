using CSWeb1PE.Data;
using CSWeb1PE.Models;
using CSWeb1PE.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CSWeb1PE.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<IdentityUser> _signInManager;

        public UsersController(
            ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewData["Roles"] = new SelectList(_dbContext.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            IdentityUser user = new IdentityUser()
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, viewModel.Paswoord);
            if (!result.Succeeded)
            {
                ViewData["Error"] = result.Errors.First().Description;
                ViewData["Roles"] = new SelectList(_dbContext.Roles, "Name", "Name");
                return View(viewModel);
            }

            Gebruiker gebruiker = new Gebruiker()
            {
                Email = viewModel.Email,
                Naam = viewModel.Naam,
                Voornaam = viewModel.Voornaam,
                TijdelijkeRol = viewModel.Role,
            };
            _dbContext.Add(gebruiker);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Paswoord, true, false);
            if (!result.Succeeded)
            {
                ViewData["Error"] = "Email of paswoord was incorrect!";
                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
