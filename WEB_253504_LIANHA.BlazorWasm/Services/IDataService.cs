using WEB_253504_LIANHA.Domain.Entities;

namespace WEB_253504_LIANHA.BlazorWasm.Services
{
    public interface IDataService
    {
        event Action DataLoaded;
        // Список категорий объектов
        List<AutomobileCategory> Categories { get; set; }
        //Список объектов
        List<Automobile> Automobiles { get; set; }
        // Признак успешного ответа на запрос к Api
        bool Success { get; set; }
        // Сообщение об ошибке
        string ErrorMessage { get; set; }
        // Количество страниц списка
        int TotalPages { get; set; }
        // Номер текущей страницы
        int CurrentPage { get; set; }
        // Фильтр по категории
        AutomobileCategory? SelectedCategory { get; set; }

        public Task GetProductListAsync(int pageNo = 0);

        public Task GetCategoryListAsync();
    }
}
