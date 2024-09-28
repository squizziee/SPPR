using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA.API.Data;
using WEB_253504_LIANHA.API.Services;
using WEB_253504_LIANHA.API.Services.CategoryService;
using WEB_253504_LIANHA.API.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<IAutomobileCategoryService, AutomobileCategoryService>();
builder.Services.AddScoped<IAutomobileService, AutomobileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// await DbInitializer.SeedData(app);

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
