using Microsoft.AspNetCore.Identity;
using PhotoManager.IdentityServer.Core.Models;
using System;
using System.Linq;

namespace PhotoManager.IdentityServer.Data
{
    public static class SeedIdentityDb
    {
        public static void SeedRoles(RoleManager<UserRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new UserRole("Administrator");
                _ = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("RegularUser").Result)
            {
                var role = new UserRole("RegularUser");
                _ = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("PaidUser").Result)
            {
                var role = new UserRole("PaidUser");
                _ = roleManager.CreateAsync(role).Result;
            }
        }
        public static void SeedUsers(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user1 = new User()
                {
                    Id = new Guid("a2dc09dc-448d-424f-9180-5dbb96d873d9"),
                    UserName = "user1@user1.com",
                    Email = "user1@user1.com",
                    Year = 0

                };
                var user2 = new User()
                {
                    Id = new Guid("a777bde3-68da-48d3-b0bd-38c880cf0af8"),
                    UserName = "user2@user2.com",
                    Email = "user2@user2.com",
                    Year = 0
                };
                var user3 = new User()
                {
                    Id = new Guid("8b98f598-a728-4263-a920-ffc8e3b9f86e"),
                    UserName = "user3@user3.com",
                    Email = "user3@user3.com",
                    Year = 0
                };
                var user4 = new User()
                {
                    Id = new Guid("e51c9a88-17fd-4130-8a99-b651b8967cfa"),
                    UserName = "user4@user4.com",
                    Email = "user4@user4.com",
                    Year = 0
                };

                _ = userManager.CreateAsync(user1, "user1").Result;
                _ = userManager.AddToRoleAsync(user1, "RegularUser").Result;
                _ = userManager.CreateAsync(user2, "user2").Result;
                _ = userManager.AddToRoleAsync(user2, "RegularUser").Result;
                _ = userManager.CreateAsync(user3, "user3").Result;
                _ = userManager.AddToRoleAsync(user3, "RegularUser").Result;
                _ = userManager.CreateAsync(user4, "user4").Result;
                _ = userManager.AddToRoleAsync(user4, "RegularUser").Result;
            }
        }
    }
}
