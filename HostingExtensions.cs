using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();
        builder.Services.AddIdentityServer()
           .AddInMemoryIdentityResources(Config.IdentityResources)
           .AddInMemoryApiScopes(Config.ApiScopes)
           .AddInMemoryClients(Config.Clients)
           .AddTestUsers(TestUsers.Users);

        //builder.Services.AddIdentityServer(options =>
        //    {
        //        options.Events.RaiseErrorEvents = true;
        //        options.Events.RaiseInformationEvents = true;
        //        options.Events.RaiseFailureEvents = true;
        //        options.Events.RaiseSuccessEvents = true;

        //        options.EmitStaticAudienceClaim = true;
        //    })
        //    .AddTestUsers(Config.Users)
        //    .AddInMemoryClients(Config.Clients)
        //    .AddInMemoryApiResources(Config.ApiResources)
        //    .AddInMemoryApiScopes(Config.ApiScopes)
        //    .AddInMemoryIdentityResources(Config.IdentityResources)
        //     .AddDeveloperSigningCredential(); 

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        
            
        app.UseIdentityServer();
        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();
        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
