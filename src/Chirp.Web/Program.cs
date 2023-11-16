using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Chirp.Infrastructure;
using Chirp.Core;
using Chirp.Web;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var dbPath = Path.GetTempPath() + "chirp.db";
builder.Configuration["ConnectionStrings:ChirpSQlite"] = $"Data Source={dbPath}";
Console.WriteLine(dbPath);

var conStr = builder.Configuration.GetConnectionString("ChirpSQlite");

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddSingleton<ICheepService, CheepService>();
builder.Services.AddTransient<ICheepRepository, CheepRepository>();
builder.Services.AddDbContext<ChirpContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
);

builder.Services.AddDefaultIdentity<Author>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ChirpContext>();
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

app.MapRazorPages();

app.Run();

public partial class Program { }

