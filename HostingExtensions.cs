using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using Duende.IdentityServer.EntityFramework;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;

namespace IdentityServer;

internal static class HostingExtensions
{
    private static void InitializeDatabase(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
        var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
        builder.Services.AddRazorPages();


        builder.Services.AddIdentityServer()
            // this adds the config data from DB (clients, resources, CORS)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
            }).AddTestUsers(TestUsers.Users); ;

         

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        InitializeDatabase(app);

        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
    //public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    //{
    //    // uncomment if you want to add a UI
    //    builder.Services.AddRazorPages();
    //    builder.Services.AddIdentityServer()
    //       .AddInMemoryIdentityResources(Config.IdentityResources)
    //       .AddInMemoryApiScopes(Config.ApiScopes)
    //       .AddInMemoryClients(Config.Clients)
    //       .AddTestUsers(TestUsers.Users);


    //    return builder.Build();
    //}

    //public static WebApplication ConfigurePipeline(this WebApplication app)
    //{ 
    //    app.UseSerilogRequestLogging();

    //    if (app.Environment.IsDevelopment())
    //    {
    //        app.UseDeveloperExceptionPage();
    //    }



    //    app.UseIdentityServer();
    //    // uncomment if you want to add a UI
    //    app.UseStaticFiles();
    //    app.UseRouting();
    //    // uncomment if you want to add a UI
    //    app.UseAuthorization();
    //    app.MapRazorPages().RequireAuthorization();

    //    return app;
    //}
}
