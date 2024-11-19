var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Für Endpunkte erforderlich
builder.Services.AddSwaggerGen(); // Swagger-Dienst hinzufügen

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); // Swagger JSON-Dokument
    app.UseSwaggerUI(); // Swagger UI
}

// app.UseHttpsRedirection();
app.MapControllers();

Console.WriteLine("Die Anwendung wurde gestartet.");
app.Run();


