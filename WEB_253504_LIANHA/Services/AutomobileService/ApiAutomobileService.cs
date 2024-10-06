using Microsoft.AspNetCore.Http;
using NuGet.Protocol;
using System.Text;
using System.Text.Json;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;
using WEB_253504_LIANHA.Services.Authentication;

namespace WEB_253504_LIANHA.Services.AutomobileService
{
	public class ApiAutomobileService : IAutomobileService
	{
		private readonly HttpClient _httpClient;
		private readonly IFileService _fileService;
		private readonly int _pageSize;
		private readonly IConfiguration _configuration;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly ILogger<ApiAutomobileService> _logger;
		private readonly ITokenAccessor _tokenAccessor;

		public ApiAutomobileService(HttpClient httpClient,
			IConfiguration configuration,
			ILogger<ApiAutomobileService> logger, 
			IFileService fileService, 
			ITokenAccessor tokenAcessor)
		{
			_httpClient = httpClient;
			_fileService = fileService;
			_configuration = configuration;
			_pageSize = configuration.GetValue<int>("ItemsPerPage");
			_serializerOptions = new JsonSerializerOptions()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			_logger = logger;
			_tokenAccessor = tokenAcessor;

		}

		public async Task<ResponseData<Automobile>> CreateAutomobileAsync(Automobile automobile, IFormFile? formFile)
		{
            if (formFile != null)
            {
                var url = await SaveImageAsync(0, formFile!);
                automobile.ImageUrl = url.Data;
            }

			await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "automobiles");
			var response = await _httpClient.PostAsJsonAsync(uri, automobile, _serializerOptions);
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadFromJsonAsync<ResponseData<Automobile>>(_serializerOptions);
				return data!;
			}
			_logger.LogError($"-----> object not created. Error:{response.StatusCode.ToString()}");
			return ResponseData<Automobile>.Error($"Объект не добавлен. Error:{response.StatusCode.ToString()}");
		}

		public async Task DeleteAutomobileAsync(int id)
		{
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "automobiles/" + id.ToString());
			var response = await _httpClient.DeleteAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				return;
			}
			_logger.LogError($"-----> object not created. Error:{response.StatusCode.ToString()}");
			return;
		}

		public async Task<ResponseData<Automobile>> GetAutomobileByIdAsync(int id)
		{
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}automobiles/");
			urlString.Append(id);
			var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
			if (response.IsSuccessStatusCode)
			{
				try
				{
					var decoded = await response.Content.ReadFromJsonAsync<ResponseData<Automobile>>(_serializerOptions);
					_logger.LogInformation($"-----> Info: {decoded}");
					return decoded!;
				}
				catch (JsonException ex)
				{
					_logger.LogError($"-----> Ошибка: {ex.Message}");
					return ResponseData<Automobile>.Error($"Ошибка: {ex.Message}");
				}
			}
			_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
			return ResponseData<Automobile>.Error($"Данные не получены от сервера. Error:{response.StatusCode}");
		}

		public async Task<ResponseData<ListModel<Automobile>>> GetAutomobileListAsync(string? categoryNormalizedName, int pageNo = 0)
		{
			var pageSize = _configuration.GetValue<int>("ItemsPerPage");

			var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}automobiles/categories/");
			if (categoryNormalizedName != null) urlString.Append($"{categoryNormalizedName}/");
			else urlString.Append($"all");

			if (pageNo > 0)
			{
				urlString.Append(QueryString.Create("pageno", pageNo.ToString()));
				urlString.Append('&');
			}

			urlString.Append(QueryString.Create("pagesize", pageSize.ToString()));

			var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
			if (response.IsSuccessStatusCode)
			{
				try
				{
					_logger.LogInformation($"-----> Info: {response.Content.ToJson()}");
					return (await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Automobile>>>(_serializerOptions))!;
				}
				catch (JsonException ex)
				{
					_logger.LogError($"-----> Ошибка: {ex.Message}");
					return ResponseData<ListModel<Automobile>>.Error($"Ошибка: {ex.Message}");
				}
			}
			_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
			return ResponseData<ListModel<Automobile>>.Error($"Данные не получены от сервера. Error:{response.StatusCode}");
		}

		public async Task<ResponseData<ListModel<Automobile>>> GetAutomobileListAsync()
		{
			var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}automobiles/");
			var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
			if (response.IsSuccessStatusCode)
			{
				try
				{
					_logger.LogInformation($"-----> Info: {response.Content.ToJson()}");
					return (await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Automobile>>>(_serializerOptions))!;
				}
				catch (JsonException ex)
				{
					_logger.LogError($"-----> Ошибка: {ex.Message}");
					return ResponseData<ListModel<Automobile>>.Error($"Ошибка: {ex.Message}");
				}
			}
			_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
			return ResponseData<ListModel<Automobile>>.Error($"Данные не получены от сервера. Error:{response.StatusCode}");
		}

		public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
		{
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var url = await _fileService.SaveFileAsync(formFile);
			return ResponseData<string>.Success(url);
        }

		public async Task UpdateAutomobileAsync(int id, Automobile automobile, IFormFile? formFile)
		{
            if (automobile.ImageUrl != null && formFile != null)
            {
                await _fileService.DeleteFileAsync(automobile.ImageUrl.Split("/").Last());
                automobile.ImageUrl = null;
            }
			if (formFile != null)
			{
                var url = await SaveImageAsync(id, formFile!);
				automobile.ImageUrl = url.Data;
            }

            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}automobiles/" + id.ToString());
			var response = await _httpClient.PutAsJsonAsync(new Uri(urlString.ToString()), automobile, _serializerOptions);
			if (response.IsSuccessStatusCode)
			{
				_logger.LogInformation($"-----> Info: {response.Content.ToJson()}");
				return;
			}
			_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
		}
	}
}
