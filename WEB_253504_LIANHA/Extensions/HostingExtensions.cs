using WEB_253504_LIANHA.Services.CategoryService;
using WEB_253504_LIANHA.Services.AutomobileService;

namespace WEB_253504_LIANHA.Extensions
{
	public static class HostingExtensions
	{
		public static void RegisterCustomServices(
			this WebApplicationBuilder builder)
		{
			//builder.Services.AddScoped<IAutomobileCategoryService, MemoryAutomobileCategoryService>();
   //         builder.Services.AddScoped<IAutomobileService, MemoryAutomobileService>();
        }
	}
}
