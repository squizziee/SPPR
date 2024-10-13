using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;
using WEB_253504_LIANHA.API.Services.ProductService;
using WEB_253504_LIANHA.API.Data;
using Microsoft.EntityFrameworkCore;

namespace WEB_253504_LIANHA.API.Services
{
	public class AutomobileService : IAutomobileService
	{
		private AppDbContext _dbContext;
		public AutomobileService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public Task<ResponseData<Automobile>> CreateAutomobileAsync(Automobile automobile)
		{
			_dbContext.Automobiles.Add(automobile);
			_dbContext.SaveChanges();
            return Task.FromResult(ResponseData<Automobile>.Success(automobile));
		}

		public Task DeleteAutomobileAsync(int id)
		{
			var toRemove = _dbContext.Automobiles.Where(a => a.Id == id).First();
			_dbContext.Automobiles.Remove(toRemove);
            return Task.FromResult(_dbContext.SaveChanges());
		}

		public Task<ResponseData<Automobile>> GetAutomobileByIdAsync(int id)
		{
            var found = _dbContext.Automobiles.Where(a => a.Id == id).First();
			return Task.FromResult(ResponseData<Automobile>.Success(found));
        }

		public Task<ResponseData<ListModel<Automobile>>> GetAutomobileListAsync(string? categoryNormalizedName, int pageNo = 0, int pageSize = 3)
		{
			if (pageSize > 20) pageSize = 20;

			List<Automobile> searchResult = [];

			if (categoryNormalizedName is null)
			{
				searchResult = _dbContext.Automobiles.ToList();
			}
			else
			{
                //searchResult = _dbContext.Automobiles
                //	.Where(a => _dbContext.AutomobileCategories
                //	.Where(c => c.Id == a.CategoryId)
                //	.First().NormalizedName == categoryNormalizedName)
                //	.ToList();

				var id = _dbContext.AutomobileCategories.Where(c => c.NormalizedName == categoryNormalizedName).First().Id;

                searchResult = _dbContext.Automobiles
                    .Where(a => a.CategoryId == id)
                    .ToList();
            }


			int totalPages = searchResult.Count % pageSize == 0 ?
				searchResult.Count / pageSize :
				searchResult.Count / pageSize + 1;

			if (pageNo > totalPages - 1)
			{
				return Task.FromResult(
				ResponseData<ListModel<Automobile>>
				.Error("No such page"));
            }

			return Task.FromResult(
				ResponseData<ListModel<Automobile>>
				.Success(
					new ListModel<Automobile> { Items = searchResult.Skip(pageSize * pageNo).Take(pageSize).ToList(), CurrentPage = pageNo, TotalPages = totalPages }
				));
		}

        public Task<ResponseData<ListModel<Automobile>>> GetAutomobileListAsync()
        {
            return Task.FromResult(ResponseData<ListModel<Automobile>>.Success(new ListModel<Automobile>{ Items = _dbContext.Automobiles.ToList(), CurrentPage = 0, TotalPages = 1 }));
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAutomobileAsync(int id, Automobile automobile, IFormFile? formFile)
		{
            _dbContext.Attach(automobile).State = EntityState.Modified;	
            return Task.FromResult(_dbContext.SaveChanges());
        }
	}
}
