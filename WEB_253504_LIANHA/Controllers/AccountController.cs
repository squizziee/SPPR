using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WEB_253504_LIANHA.Models;
using WEB_253504_LIANHA.Services.Authentication;

namespace WEB_253504_LIANHA.Controllers
{
	public class AccountController : Controller
	{

		private readonly IAuthService _authService;

		public AccountController(IAuthService authService)
		{
			_authService = authService;
		}

		public IActionResult Index()
		{
			return View(new RegisterUserViewModel());
		}

		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Register(RegisterUserViewModel user)
		{
			if (ModelState.IsValid)
			{
				if (user == null)
				{
					return BadRequest();
				}
				var result = await _authService.RegisterUserAsync(user.Email, user.Password, user.Avatar);
				var tmp = Url.Action("Index", "Home");
				if (result.Result)
				{
					return Redirect(Url.Action("Index", "Home")!);
				}
				else return BadRequest(result.ErrorMessage);
			}
			return View(user);
		}

		public IActionResult LoginIndex()
		{
			return View(new LoginUserViewModel());
		}

		public async Task Login()
		{
			await HttpContext.ChallengeAsync(
				OpenIdConnectDefaults.AuthenticationScheme,
				new AuthenticationProperties
				{
					RedirectUri = Url.Action("Index", "Home")
				}
			);
		}
		[HttpPost]
		public async Task Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
				new AuthenticationProperties
				{
					RedirectUri = Url.Action("Index", "Home")
				}
			);
		}
	}
}
