using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using NeoSharovarshyna.Services.IdentityServer.Models;
using System.Security.Claims;

namespace NeoSharovarshyna.Services.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private IUserClaimsPrincipalFactory<ApplicationUser> _userClaimFactory;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;

        public ProfileService(IUserClaimsPrincipalFactory<ApplicationUser> userClaimFactory, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _userClaimFactory = userClaimFactory;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUser user = await _userManager.FindByIdAsync(sub);
            ClaimsPrincipal principal = await _userClaimFactory.CreateAsync(user);
            List<Claim> claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            if(_userManager.SupportsUserRole)
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach(var roleName in roles)
                {
                    IdentityRole role = await _roleManager.FindByNameAsync(roleName);
                    if(role != null)
                    {
                        claims.AddRange(await _roleManager.GetClaimsAsync(role));
                    }
                }
            }
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            ApplicationUser user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
