using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WEB_253504_LIANHA.Domain.Models;

namespace WEB_253504_LIANHA.Components
{
    public class CartComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Cart cart)
        {
            return View(cart);
        }
    }
}
