using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;

namespace WEB_253504_LIANHA.Services.SessionService
{
    public interface ISessionCartService
    {
        void AddToCart(Automobile automobile);

        void RemoveItems(int id);

        void ClearAll();

        Cart GetCart();
    }
}
