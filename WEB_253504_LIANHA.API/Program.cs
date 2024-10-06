using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA.API.Data;
using WEB_253504_LIANHA.API.Models;
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

var authServer = builder.Configuration
	.GetSection("AuthServer")
	.Get<AuthServerData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
	{
		o.MetadataAddress = $"{authServer!.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
		o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
		o.Audience = "account";
		o.RequireHttpsMetadata = false;
	}
);

builder.Services.AddAuthorization(opt =>
{
	opt.AddPolicy("admin", p => p.RequireRole("POWER_USER"));
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
