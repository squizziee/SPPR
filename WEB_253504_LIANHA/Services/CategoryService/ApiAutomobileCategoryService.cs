using System.Text;
using System.Text.Json;
using WEB_253504_LIANHA.Services.CategoryService;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;
using WEB_253504_LIANHA.Services.AutomobileService;

namespace WEB_253504_LIANHA.Services.CategoryService
{
    public class ApiAutomobileCategoryService : IAutomobileCategoryService
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiAutomobileService> _logger;

        public ApiAutomobileCategoryService(HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ApiAutomobileService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;

        }

        public Task<ResponseData<AutomobileCategory>> GetAutomobileCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<List<AutomobileCategory>>> GetAutomobileCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}categories/");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return (await response.Content.ReadFromJsonAsync<ResponseData<List<AutomobileCategory>>>(_serializerOptions))!;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<List<AutomobileCategory>>.Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return ResponseData<List<AutomobileCategory>>.Error($"Данные не получены от сервера. Error:{response.StatusCode}");
        }
    }
}
