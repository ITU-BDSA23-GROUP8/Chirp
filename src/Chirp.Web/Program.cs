using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Chirp.Infrastructure;
using Chirp.Core;
using Chirp.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddSingleton<ICheepService, CheepService>();
builder.Services.AddTransient<ICheepRepository, CheepRepository>();
builder.Services.AddTransient<IAuthorRepository, AuthorRepository>();
builder.Services.AddDbContext<ChirpContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
);

builder.Services.AddDefaultIdentity<Author>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ChirpContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddAuthentication()
    .AddGitHub(o =>
    {
        o.ClientId = builder.Configuration["authentication:github:clientId"];
        o.ClientSecret = builder.Configuration["authentication:github:clientSecret"];
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
    DBInitializer.SeedDatabase(context);
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

