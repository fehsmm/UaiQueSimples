using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UaiQueSimples.Web.Data;
using UaiQueSimples.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Connection string (SQL Server Express/localdb). Update if needed.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=(localdb)\\MSSQLLocalDB;Database=UaiQueSimplesDb;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Identity UI

// Seed initial roles and admin
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string[] roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var adminEmail = "admin@uaiquesimples.local";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin is null)
    {
        admin = new ApplicationUser { UserName = "admin", Email = adminEmail, EmailConfirmed = true, DisplayName = "Administrator" };
        await userManager.CreateAsync(admin, "Admin@123");
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

app.Run();