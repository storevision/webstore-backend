using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Webshop.App.src.main.Services;
using Microsoft.Extensions.FileProviders;
using Webshop.App.src.main.Services;
using Webshop.Models.DB;
using Webshop.Services;

var builder = WebApplication.CreateBuilder(args);

// Lade die Umgebungsvariablen aus der .env-Datei
Env.Load("backend.env");

// Konfiguriere die Datenbankverbindung
var connectionString = $"Host={Environment.GetEnvironmentVariable("POSTGRES_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("POSTGRES_PORT")};" + 
                       $"Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};" + 
                       $"Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};" + 
                       $"Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")}";

//Console.WriteLine($"ConnectionString: {connectionString}");

// Add services to the container.
builder.Services.AddLogging();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // necessary for the endpoint 
builder.Services.AddSwaggerGen(); // Add Swagger-Service
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<CategorieService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 
//    ApplicationDbContext.TestDatabaseConnection(context);
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); // Swagger JSON-Dokument
    app.UseSwaggerUI(); // Swagger UI
}

// route for static data's defined with "/api/assets/"
app.UseStaticFiles();
app.MapWhen(context => context.Request.Path.StartsWithSegments("/api/assets"), subApp =>
{
    subApp.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets")),
        RequestPath = "/api/assets"
    });
});

app.UseHttpsRedirection();
app.UseAuthorization();

// app.UseHttpsRedirection();
app.MapControllers();
//Der Methodenaufruf von ApplicationDbContext für eine sql abfrage


Console.WriteLine("Die Anwendung wurde gestartet.");
app.Run();


