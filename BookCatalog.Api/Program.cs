using BookCatalog.Api.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Connection String for DB PostgreSQL -- Start --
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    // Let the user know what error is coming up
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
}

builder.Services.AddDbContext<BookCatalogDbContext>(options =>
{
    // Use Npgsql provider for PostgreSQL
    options.UseNpgsql(connectionString);
});

// DB Connection -- End --

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();