using WEB_253504_LIANHA.API.Data;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;
using WEB_253504_LIANHA.API.Services;
using WEB_253504_LIANHA.API.Services.CategoryService;

namespace WEB_253504_LIANHA.API.Services
{
    public class AutomobileCategoryService : IAutomobileCategoryService
    {
        private AppDbContext _dbContext;
        public AutomobileCategoryService(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Task<ResponseData<List<AutomobileCategory>>> GetAutomobileCategoryListAsync()
        {
            var result = _dbContext.AutomobileCategories.ToList();
            return Task.FromResult(ResponseData<List<AutomobileCategory>>.Success(result));
        }

        public Task<ResponseData<AutomobileCategory>> GetAutomobileCategoryAsync(int id)
        {
            var result = _dbContext.AutomobileCategories.Where(c => c.Id == id).First();
            return Task.FromResult(ResponseData<AutomobileCategory>.Success(result));
        }
    }
}
