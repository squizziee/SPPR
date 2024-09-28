using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;

namespace WEB_253504_LIANHA.API.Services.CategoryService
{
    public interface IAutomobileCategoryService
    {
        public Task<ResponseData<List<AutomobileCategory>>> GetAutomobileCategoryListAsync();
        public Task<ResponseData<AutomobileCategory>> GetAutomobileCategoryAsync(int id);
    }
}
