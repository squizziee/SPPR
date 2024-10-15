using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;

namespace WEB_253504_LIANHA.BlazorWasm.Services
{
	public class ApiDataService : IDataService
	{
		public List<AutomobileCategory> Categories { get; set; }
		public List<Automobile> Automobiles { get; set; }
		public bool Success { get; set; }
		public string ErrorMessage { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }
		public AutomobileCategory? SelectedCategory { get; set; }

		public event Action DataLoaded;

		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly ILogger<ApiDataService> _logger;
		private readonly IAccessTokenProvider _tokenProvider;

		public ApiDataService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiDataService> logger, IAccessTokenProvider tokenProvider)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_logger = logger;
			_tokenProvider = tokenProvider;
		}

		public async Task GetCategoryListAsync()
		{
			var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}categories");


			var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadFromJsonAsync<ResponseData<List<AutomobileCategory>>>();
				Categories = data!.Data!;
				ErrorMessage = data.ErrorMessage ?? "";
			}
		}

		public async Task GetProductListAsync(int pageNo = 0)
		{
            _logger.LogWarning($"--------------------Current page: {pageNo}");
            var tokenRequest = await _tokenProvider.RequestAccessToken();
			if (!tokenRequest.TryGetToken(out var token))
			{
				return;
			}

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
			var pageSize = _configuration.GetValue<int>("ItemsPerPage");
			var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}automobiles/categories/");

			if (SelectedCategory is not null)
			{
				urlString.Append($"{SelectedCategory.NormalizedName}");
			}
			else
			{
				urlString.Append("all");
			}

			if (pageNo > 0)
			{
				urlString.Append($"?pageno={pageNo}&pagesize={pageSize}");
			}
			else
			{
				urlString.Append($"?pagesize={pageSize}");
			}
			
			var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

			if (response.IsSuccessStatusCode)
			{
				_logger.LogWarning(urlString.ToString());
				_logger.LogWarning(await response.Content.ReadAsStringAsync());
				var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Automobile>>>();
				Automobiles = data!.Data!.Items;
				Success = data.Successful;
				TotalPages = data.Data.TotalPages;
				CurrentPage = data.Data.CurrentPage;
				ErrorMessage = data.ErrorMessage ?? "";
				DataLoaded?.Invoke();
            }
		}
	}
}
