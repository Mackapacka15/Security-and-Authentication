using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SecurityAndAuth;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<IdentityDbContext>(options =>
            options.UseInMemoryDatabase("IdentityDbContext")
        );
        builder.Services.AddAuthorization();
        builder
            .Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "Admin", "User", "Manager" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        app.MapIdentityApi<IdentityUser>();

        app.MapControllers();

        app.Run();
    }
}
