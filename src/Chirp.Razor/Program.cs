using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var dbPath = Path.GetTempPath() + "./chirp.db";
builder.Configuration["ConnectionStrings:ChirpSQlite"] = $"Data Source={dbPath}";

var conStr = builder.Configuration.GetConnectionString("ChirpSQlite");

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddSingleton<ICheepService, CheepService>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddDbContext<ChirpContext>(
    options => options.UseSqlite(conStr)
);


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

app.MapRazorPages();

app.Run();

public partial class Program {}
