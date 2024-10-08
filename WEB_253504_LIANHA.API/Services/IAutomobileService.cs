﻿using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;

namespace WEB_253504_LIANHA.API.Services.ProductService
{
    public interface IAutomobileService
    {
        public Task<ResponseData<ListModel<Automobile>>> GetAutomobileListAsync(string?
            categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        public Task<ResponseData<ListModel<Automobile>>> GetAutomobileListAsync();
        public Task<ResponseData<Automobile>> GetAutomobileByIdAsync(int id);
        public Task UpdateAutomobileAsync(int id, Automobile product, IFormFile? formFile);
        public Task DeleteAutomobileAsync(int id);
        public Task<ResponseData<Automobile>> CreateAutomobileAsync(Automobile automobile);
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
