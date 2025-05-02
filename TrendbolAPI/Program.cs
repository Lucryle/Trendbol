using Microsoft.EntityFrameworkCore;
using TrendbolAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Controller desteği
builder.Services.AddControllers();

// Swagger (API test aracı)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TrendbolContext'i EF Core ile servise ekle
builder.Services.AddDbContext<TrendbolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Geliştirme ortamında Swagger'ı aktif et
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
