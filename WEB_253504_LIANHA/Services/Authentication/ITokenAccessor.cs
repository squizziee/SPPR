namespace WEB_253504_LIANHA.Services.Authentication
{
    public interface ITokenAccessor
    {
        Task<string> GetAccessTokenAsync();
        Task SetAuthorizationHeaderAsync(HttpClient httpClient);
    }
}
