using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }
        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            if(await userManager.FindByEmailAsync("admin@bookstore.co.nz") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin@bookstore.co.nz",
                    Email = "admin@bookstore.co.nz"
                };
                var result = await userManager.CreateAsync(user, "Fr33d0m!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
            if (await userManager.FindByEmailAsync("flic@felicitywinter.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "flic@felicitywinter.com",
                    Email = "flic@felicitywinter.com"
                };
                var result = await userManager.CreateAsync(user, "Fr33d0m!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
            if (await userManager.FindByEmailAsync("felicity.sommers@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "felicity.sommers@gmail.com",
                    Email = "felicity.sommers@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "Fr33d0m!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
            }
        }
        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if(!await roleManager.RoleExistsAsync("Administrator"))
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                var result = await roleManager.CreateAsync(role);
            }
            
            if(!await roleManager.RoleExistsAsync("Staff"))
            {
                var role = new IdentityRole
                {
                    Name = "Staff"
                };
                var result = await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Helpdesk1"))
            {
                var role = new IdentityRole
                {
                    Name = "Helpdesk1"
                };
                var result = await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };
                var result = await roleManager.CreateAsync(role);
            }
        }

    }
}
