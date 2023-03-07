using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeoSharovarshyna.Services.IdentityServer.DbContexts;
using NeoSharovarshyna.Services.IdentityServer.Models;
using Serilog;
using System.Security.Claims;
using NeoSharovarshyna.Services.IdentityServer.Tools;

namespace NeoSharovarshyna.Services.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (roleMgr.FindByNameAsync(Role.Admin).Result == null)
                {
                    roleMgr.CreateAsync(new IdentityRole(Role.Admin)).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(new IdentityRole(Role.Customer)).GetAwaiter().GetResult();
                }
                else return;
                var admin = new ApplicationUser()
                {
                    UserName = "admin1@gmail.com",
                    Email = "admin1@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "111111111111",
                    FirstName = "Ben",
                    LastName = "Admin"
                };

                userMgr.CreateAsync(admin, "Admin123*").GetAwaiter().GetResult();
                userMgr.AddToRoleAsync(admin, Role.Admin).GetAwaiter().GetResult();
                
                var temp = userMgr.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, admin.FirstName + " " + admin.LastName),
                    new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                    new Claim(JwtClaimTypes.Role, Role.Admin),

                }).Result;

                var customer = new ApplicationUser()
                {
                    UserName = "customer1@gmail.com",
                    Email = "customer1@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "111111111111",
                    FirstName = "Ben",
                    LastName = "Cust"
                };

                userMgr.CreateAsync(customer, "Customer123*").GetAwaiter().GetResult();
                userMgr.AddToRoleAsync(customer, Role.Customer).GetAwaiter().GetResult();

                var temp1 = userMgr.AddClaimsAsync(customer, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, customer.FirstName + " " + customer.LastName),
                    new Claim(JwtClaimTypes.GivenName, customer.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, customer.LastName),
                    new Claim(JwtClaimTypes.Role, Role.Customer),

                }).Result;
            }
        }   
    }
}