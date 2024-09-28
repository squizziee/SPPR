using Microsoft.AspNetCore.Mvc;

namespace WEB_253504_LIANHA
{
    public static class ApplicationConfiguration
    {
        private static IConfiguration _configuration;

        static ApplicationConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
        }

        public static string GetSetting(string key)
        {
            return _configuration[key]!;
        }
    }
}
