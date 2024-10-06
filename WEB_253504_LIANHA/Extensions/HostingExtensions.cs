using WEB_253504_LIANHA.Services.CategoryService;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.HelperClasses;

namespace WEB_253504_LIANHA.Extensions
{
	public static class HostingExtensions
	{
		public static void RegisterCustomServices(
			this WebApplicationBuilder builder)
		{
            builder.Services
				.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
        }
	}
}
