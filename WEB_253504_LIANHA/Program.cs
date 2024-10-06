using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using WEB_253504_LIANHA;
using WEB_253504_LIANHA.Extensions;
using WEB_253504_LIANHA.HelperClasses;
using WEB_253504_LIANHA.Services;
using WEB_253504_LIANHA.Services.Authentication;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Services.CategoryService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.RegisterCustomServices();

var uriData = new UriData
{
	ApiUri = builder.Configuration.GetSection("UriData").GetValue<string>("ApiUri")!
};

builder.Services
	.AddHttpClient<IFileService, ApiFileService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services
	.AddHttpClient<IAutomobileService, ApiAutomobileService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services
	.AddHttpClient<IAutomobileCategoryService, ApiAutomobileCategoryService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ITokenAccessor, KeycloakTokenAccessor>();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
