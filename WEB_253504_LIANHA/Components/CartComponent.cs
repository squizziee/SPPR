using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace WEB_253504_LIANHA.Components
{
    public class CartComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();// new HtmlString("00,0 руб <i class=\"fa-solid fa-cart-shopping\"></i> (0)");
        }
    }
}
