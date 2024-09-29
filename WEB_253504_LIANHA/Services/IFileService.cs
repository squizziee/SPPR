namespace WEB_253504_LIANHA.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile formFile);
        Task DeleteFileAsync(string fileName);
    }
}
