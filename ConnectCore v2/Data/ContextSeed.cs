using ConnectCore_v2.Models;
using EnumsNET;
using Microsoft.AspNetCore.Identity;

namespace ConnectCore_v2.Data
{
    public class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.createUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.updateUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.createShifts.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.updateShifts.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.basic.ToString()));
        }
    }
}
