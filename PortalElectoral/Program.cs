using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using PortalElectoral.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Portal Electoral API",
        Version = "v1",
        Description = "API educativa de gestión electoral. Los partidos, candidatos y resultados de demostración son ficticios."
    });
});
string connectionString =
    builder.Configuration.GetConnectionString("ElectoralMySql")?? throw new InvalidOperationException("No se ha configurado la conexión ElectoralMySql.");
    builder.Services.AddDbContext<ElectoralDbContext>(options =>
{
    options.UseMySQL(connectionString);
    options.EnableDetailedErrors();
    options.LogTo(
    Console.WriteLine,
    LogLevel.Information);
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","Portal Electoral API v1");
    });
}
app.UseHttpsRedirection();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider
    .GetRequiredService<ElectoralDbContext>();
    await DemoDataSeeder.InicializarAsync(db);
}

app.Run();