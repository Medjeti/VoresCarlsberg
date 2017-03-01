using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using VoresCarlsberg.Application.Services;
using VoresCarlsberg.Web.Models;

namespace VoresCarlsberg.Web.ApiControllers
{
	[RoutePrefix("api/guest")]
	public class GuestController : ApiController
	{
		[HttpPost]
		[Route("addguest")]
		public GuestModel AddGuest(GuestModel guest)
		{	
			return guest;
		}

		// ---------------------------------------------------------------------------

		[HttpPost]
		[Route("login")]
		public IHttpActionResult Login(LoginModel login)
		{
			var loginService = new LoginService();
			var loggedIn = loginService.LoginUser(login);

			if (loggedIn)
			{
				FormsAuthentication.SetAuthCookie(login.EmployeeNo, false);

				var guestService = new GuestService();
				var guest = guestService.GetGuest(login);

				return Ok(new { Guest = guest });
			}
			
			return Unauthorized();
			
		}

		// ---------------------------------------------------------------------------

		[HttpPost]
		[Route("submit")]
		public void Submit(GuestModel guest)
		{
			var guestService = new GuestService();
			guestService.SaveGuest(guest);
		}
	}
}
