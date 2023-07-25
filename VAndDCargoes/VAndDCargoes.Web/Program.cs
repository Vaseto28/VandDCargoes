using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Services.Contracts;
using static VAndDCargoes.Web.Infrastructure.Extentions.WebApplicationBuilderExtentions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<VAndDCargoesDbContext>(options =>
    options.UseSqlServer(connectionString, x => x.MigrationsAssembly("VAndDCargoes.Web")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");

    options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequreDigit");
    options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
    options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
})
.AddEntityFrameworkStores<VAndDCargoesDbContext>();

builder.Services.AddApplicationServices(typeof(IDriverService));

builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
app.MapRazorPages();

app.Run();

