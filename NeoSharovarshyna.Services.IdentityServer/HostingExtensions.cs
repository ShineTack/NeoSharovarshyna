using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeoSharovarshyna.Services.IdentityServer.DbContexts;
using NeoSharovarshyna.Services.IdentityServer.Models;
using NeoSharovarshyna.Services.IdentityServer.Services;
using NeoSharovarshyna.Services.IdentityServer.Tools;
using Serilog;

namespace NeoSharovarshyna.Services.IdentityServer
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var identityBuilder = builder.Services.AddIdentityServer(opt =>
            {
                opt.Events.RaiseErrorEvents = true;
                opt.Events.RaiseInformationEvents = true;
                opt.Events.RaiseSuccessEvents = true;
                opt.Events.RaiseFailureEvents = true;
                opt.EmitStaticAudienceClaim = true;
            }).AddInMemoryIdentityResources(StaticData.IdentityResources)
            .AddInMemoryApiScopes(StaticData.ApiScopes)
            .AddInMemoryClients(StaticData.Clients)
            .AddAspNetIdentity<ApplicationUser>();

            identityBuilder.AddDeveloperSigningCredential();
            builder.Services.AddScoped<IProfileService, ProfileService>();


            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.MapRazorPages()
                .RequireAuthorization();

            return app;
        }
    }
}