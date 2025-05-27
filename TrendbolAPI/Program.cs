using Microsoft.EntityFrameworkCore;
using TrendbolAPI.Data;
using TrendbolAPI.Repositories.Implementations;
using TrendbolAPI.Repositories.Interfaces;
using TrendbolAPI.Services.Implementations;
using TrendbolAPI.Services.Interfaces;
using TrendbolAPI.Factories;

var builder = WebApplication.CreateBuilder(args);

// MVC - Model View Controller , bizde view yok 
builder.Services.AddControllers();

// Database 
builder.Services.AddDbContext<TrendbolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency injections, Transient = yeni instance , Scoped = her request için yeni instance, Singleton = 1 instance   
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductFactory, ProductFactory>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
