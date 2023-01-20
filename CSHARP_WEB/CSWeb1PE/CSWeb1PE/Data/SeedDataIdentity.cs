using Microsoft.AspNetCore.Identity;

namespace CSWeb1PE.Data
{
    public class SeedDataIdentity
    {
        public static async Task EnsurePopulated(WebApplication application)
        {
            using (IServiceScope scope = application.Services.CreateScope())
            {
                ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!dbContext.UserRoles.Any())
                {
                    // Roles
                    IdentityRole studentRole = new IdentityRole()
                    {
                        Name = "Student",
                    };
                    IdentityResult result = await roleManager.CreateAsync(studentRole);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }

                    IdentityRole adminRole = new IdentityRole()
                    {
                        Name = "Admin",
                    };
                    result = await roleManager.CreateAsync(adminRole);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }

                    IdentityRole lectorRole = new IdentityRole()
                    {
                        Name = "Lector",
                    };
                    result = await roleManager.CreateAsync(lectorRole);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }

                    IdentityRole geenRole = new IdentityRole()
                    {
                        Name = "Geen",
                    };
                    result = await roleManager.CreateAsync(geenRole);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }

                    // Users
                    IdentityUser student = new IdentityUser()
                    {
                        UserName = "student@pxl.be",
                        Email = "student@pxl.be",
                    };
                    result = await userManager.CreateAsync(student, "Student123!");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }
                    result = await userManager.AddToRoleAsync(student, "Student");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }

                    IdentityUser admin = new IdentityUser()
                    {
                        UserName = "admin@pxl.be",
                        Email = "admin@pxl.be",
                    };
                    result = await userManager.CreateAsync(admin, "Admin456!");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }
                    result = await userManager.AddToRoleAsync(admin, "Admin");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }

                    IdentityUser daan = new IdentityUser()
                    {
                        UserName = "daan.vanluijk@student.pxl.be",
                        Email = "daan.vanluijk@student.pxl.be",
                    };
                    result = await userManager.CreateAsync(daan, "Daan123!");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }
                    result = await userManager.AddToRoleAsync(daan, "Student");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }

                    IdentityUser lector = new IdentityUser()
                    {
                        UserName = "kristof.palmaers@pxl.be",
                        Email = "kristof.palmaers@pxl.be",
                    };
                    result = await userManager.CreateAsync(lector, "Lector123!");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }
                    result = await userManager.AddToRoleAsync(lector, "Lector");
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Code);
                    }
                }
            }
        }
    }
}
