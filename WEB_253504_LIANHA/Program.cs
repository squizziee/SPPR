using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using WEB_253504_LIANHA;
using WEB_253504_LIANHA.Extensions;
using WEB_253504_LIANHA.HelperClasses;
using WEB_253504_LIANHA.Middleware;
using WEB_253504_LIANHA.Services;
using WEB_253504_LIANHA.Services.Authentication;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Services.CategoryService;
using WEB_253504_LIANHA.Services.SessionService;

var builder = WebApplication.CreateBuilder(args);

//var configuration = new ConfigurationBuilder()
//        .SetBasePath(Directory.GetCurrentDirectory())
//        .AddJsonFile("appsettings.json")
//        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
//        .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddSerilog();

builder.Host.UseSerilog();

Log.Logger.Information("[Started logging...]");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.RegisterCustomServices();

var uriData = new UriData
{
	ApiUri = builder.Configuration.GetSection("UriData").GetValue<string>("ApiUri")!
};

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services
	.AddHttpClient<IFileService, ApiFileService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services
	.AddHttpClient<IAutomobileService, ApiAutomobileService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services
	.AddHttpClient<IAutomobileCategoryService, ApiAutomobileCategoryService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddScoped<ITokenAccessor, KeycloakTokenAccessor>();

builder.Services.AddScoped<ISessionCartService, SessionCartService>();

//builder.Services.AddHttpClient<ISessionCartService, SessionCartService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services
    .AddHttpClient<IAuthService, KeycloakAuthService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddRazorPages();

//builder.Services.AddTransient<IAutomobileService, ApiAutomobileService>();
//builder.Services.AddTransient<IAutomobileCategoryService, ApiAutomobileCategoryService>();

var keycloakData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultScheme =
		CookieAuthenticationDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme =
		OpenIdConnectDefaults.AuthenticationScheme;
	})
	.AddCookie()
	.AddJwtBearer()
	.AddOpenIdConnect(options =>
	{
		options.Authority = $"{keycloakData!.Host}/auth/realms/{keycloakData.Realm}";
		options.ClientId = keycloakData.ClientId;
		options.ClientSecret = keycloakData.ClientSecret;
		options.ResponseType = OpenIdConnectResponseType.Code;
		options.Scope.Add("openid");
		options.SaveTokens = true;
		options.RequireHttpsMetadata = false;
		options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
	}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseMiddleware<LoggingMiddleware>();

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
