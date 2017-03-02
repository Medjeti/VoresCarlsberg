using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using VoresCarlsberg.Application.Services;
using VoresCarlsberg.Web.Models;

namespace VoresCarlsberg.Web.ApiControllers
{
	[System.Web.Http.RoutePrefix("api/guest")]
	public class GuestController : ApiController
	{
		[System.Web.Http.HttpPost]
		[System.Web.Http.Route("addguest")]
		public GuestModel AddGuest(GuestModel guest)
		{	
			return guest;
		}

		// ---------------------------------------------------------------------------

		[System.Web.Http.HttpPost]
		[System.Web.Http.Route("login")]
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

		[System.Web.Http.HttpPost]
		[System.Web.Http.Route("submit")]
		public void Submit(GuestModel guest)
		{
			var guestService = new GuestService();
			guestService.SaveGuest(guest);
		}

		// ---------------------------------------------------------------------------

		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("export")]
		public HttpResponseMessage Export()
		{
			var exportService = new ExportToExcelService();
			exportService.Generate();

			//Clear the response and add the content types and headers to it.
			//var response = HttpContext.Current.Response;
			//response.Clear();
			//response.Buffer = true;
			//response.AddHeader("content-disposition", "attachment;filename=MyReport.xls");
			//response.Charset = "";
			//response.ContentType = "application/vnd.ms-excel";

			var path = HttpContext.Current.Server.MapPath("/tmp/tilmeldinger.xslx");
			var stream = new FileStream(path, FileMode.Open);

			var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
			//httpResponse.Content = new ByteArrayContent(bytes);
			httpResponse.Content = new StreamContent(stream);
			httpResponse.Content.Headers.ContentType =
				new MediaTypeHeaderValue("application/octet-stream");
			httpResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
			{
				FileName = "tilmeldinger.xlsx"
			};

			return httpResponse;
		}

	}
}
