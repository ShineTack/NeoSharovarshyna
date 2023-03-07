using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using static NeoSharovarshyna.Services.IdentityServer.Pages.Login.ViewModel;
using NeoSharovarshyna.Services.IdentityServer.Pages.Account;
using NeoSharovarshyna.Services.IdentityServer.Pages.Login;
using Microsoft.AspNetCore.Authorization;
using Duende.IdentityServer.Events;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using NeoSharovarshyna.Services.IdentityServer.Models;
using System.Security.Claims;

namespace NeoSharovarshyna.Services.IdentityServer.Pages.Account.Registration
{
    [AllowAnonymous]
    public class Index : PageModel
    {
        private string _returnUrl;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public Index(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IIdentityServerInteractionService interaction, IClientStore clientStore, IAuthenticationSchemeProvider schemeProvider, IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public async Task<IActionResult> OnGet(string returnUrl)
        {
            _returnUrl = returnUrl;
            List<string> roles = new List<string>();
            roles.Add("Admin");
            roles.Add("Customer");
            ViewData.Add("returnUrl", returnUrl);
            ViewData.Add("roles", roles);
            // build a model so we know what to show on the reg page
            RegisterViewModel = await BuildRegisterViewModelAsync(returnUrl);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    UserName = RegisterViewModel.Username,
                    Email = RegisterViewModel.Email,
                    EmailConfirmed = true,
                    FirstName = RegisterViewModel.FirstName,
                    LastName = RegisterViewModel.LastName
                };

                var result = await _userManager.CreateAsync(user, RegisterViewModel.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(RegisterViewModel.RoleName).GetAwaiter().GetResult())
                    {
                        var userRole = new IdentityRole
                        {
                            Name = RegisterViewModel.RoleName,
                            NormalizedName = RegisterViewModel.RoleName,

                        };
                        await _roleManager.CreateAsync(userRole);
                    }

                    await _userManager.AddToRoleAsync(user, RegisterViewModel.RoleName);

                    await _userManager.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Name, RegisterViewModel.Username),
                            new Claim(JwtClaimTypes.Email, RegisterViewModel.Email),
                            new Claim(JwtClaimTypes.FamilyName, RegisterViewModel.FirstName),
                            new Claim(JwtClaimTypes.GivenName, RegisterViewModel.LastName),
                            new Claim(JwtClaimTypes.WebSite, "http://"+RegisterViewModel.Username+".com"),
                        new Claim(JwtClaimTypes.Role,"User") });

                    var context = await _interaction.GetAuthorizationContextAsync(RegisterViewModel.ReturnUrl);
                    var loginresult = await _signInManager.PasswordSignInAsync(RegisterViewModel.Username, RegisterViewModel.Password, false, lockoutOnFailure: true);
                    if (loginresult.Succeeded)
                    {
                        var checkuser = await _userManager.FindByNameAsync(RegisterViewModel.Username);
                        await _events.RaiseAsync(new UserLoginSuccessEvent(checkuser.UserName, checkuser.Id, checkuser.UserName, clientId: context?.Client.ClientId));

                        if (context != null)
                        {
                            if (context.IsNativeClient())
                            {
                                // The client is native, so this change in how to
                                // return the response is for better UX for the end user.
                                return this.LoadingPage(RegisterViewModel.ReturnUrl);
                            }

                            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                            return Redirect(RegisterViewModel.ReturnUrl);
                        }

                        // request for a local page
                        if (Url.IsLocalUrl(RegisterViewModel.ReturnUrl))
                        {
                            return Redirect(RegisterViewModel.ReturnUrl);
                        }
                        else if (string.IsNullOrEmpty(RegisterViewModel.ReturnUrl))
                        {
                            return Redirect("~/");
                        }
                        else
                        {
                            // user might have clicked on a malicious link - should be logged
                            throw new Exception("invalid return URL");
                        }
                    }
                }
            }
            return Page();
        }

        private async Task<RegisterViewModel> BuildRegisterViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new RegisterViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new RegisterViewModel
            {
                AllowRememberLogin = LoginOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToList()
            };
        }
    }
}
