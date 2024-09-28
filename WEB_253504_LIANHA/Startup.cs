using Microsoft.AspNetCore.Mvc;

namespace WEB_253504_LIANHA
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
