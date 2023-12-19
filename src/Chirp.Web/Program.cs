using Microsoft.EntityFrameworkCore;
using Chirp.Infrastructure;
using Chirp.Core;
using Chirp.Web;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

/// <summary>
/// Startup code for Chirp application. 
/// This is written in top-level statements.
/// First the WebApplication is configured, and then it is built. Lastly it is run. 
/// </summary>

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Registering services with DI (Dependency Injection) and their implementations. 
// Used when routing requests a service.
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();

builder.Services.AddDbContext<ChirpContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
);

// Sets the data model Author as the identity for ASP.NET Core Identity
builder.Services.AddDefaultIdentity<Author>(
    options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ChirpContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});


// Adds and configures GitHub OAuth to authentication
builder.Services.AddAuthentication()
    .AddGitHub(o =>
    {
        o.ClientId = builder.Configuration["authentication:github:clientId"]!;
        o.ClientSecret = builder.Configuration["authentication:github:clientSecret"]!;
        o.CallbackPath = "/signin-github";
        o.Scope.Add("user:email"); // important to get read access to email from GitHub
        o.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
        o.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ChirpContext>();
    context.Database.EnsureCreated();
    DBInitializer.SeedDatabase(context); // Seed database with sample data
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});

app.MapRazorPages();

app.Run();

public partial class Program { }

