using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253504_LIANHA.Domain.Models;
using WEB_253504_LIANHA.Extensions;
using WEB_253504_LIANHA.Services.AutomobileService;
using WEB_253504_LIANHA.Services.SessionService;

namespace WEB_253504_LIANHA.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private IAutomobileService _automobileService;
		private ISessionCartService _sessionCartService;

		public CartController(IAutomobileService automobileService, ISessionCartService sessionCartService)
		{
			_automobileService = automobileService;
			_sessionCartService = sessionCartService;
		}

		public IActionResult Index()
		{
            Cart cart = _sessionCartService.GetCart();
            return View(cart);
		}

		[Route("[controller]/add/{id:int}")]
		public async Task<ActionResult> Add(int id, string returnUrl)
		{
			var response = await _automobileService.GetAutomobileByIdAsync(id);
			if (response.Successful)
			{
				_sessionCartService.AddToCart(response.Data!);
			}
			return Redirect(returnUrl);
		}

        [Route("[controller]/delete/{id:int}")]
        public ActionResult Delete(int id, string returnUrl)
        {
			_sessionCartService.RemoveItems(id);
            return Redirect(returnUrl);
        }
    }
}
