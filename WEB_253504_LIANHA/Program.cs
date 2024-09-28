using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA;
using WEB_253504_LIANHA.API.Data;
using WEB_253504_LIANHA.Extensions;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Services.CategoryService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// builder.RegisterCustomServices();

var uriData = new UriData
{
    ApiUri = builder.Configuration.GetSection("UriData").GetValue<string>("ApiUri")!
};


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

builder.Services
    .AddHttpClient<IAutomobileService, ApiAutomobileService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services
    .AddHttpClient<IAutomobileCategoryService, ApiAutomobileCategoryService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddRazorPages();

//builder.Services.AddTransient<IAutomobileService, ApiAutomobileService>();
//builder.Services.AddTransient<IAutomobileCategoryService, ApiAutomobileCategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
