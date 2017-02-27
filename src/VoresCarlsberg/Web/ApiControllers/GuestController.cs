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
using VoresCarlsberg.Application.Services;
using VoresCarlsberg.Web.Models;

namespace VoresCarlsberg.Web.ApiControllers
{
	[RoutePrefix("api/guest")]
	public class GuestController : ApiController
	{
		[HttpPost]
		[Route("addguest")]
		public bool AddGuest(GuestModel model)
		{
			// Send reset password e-mail
			// --------------------------
			var templatePath = "/templates/requestmeeting.html";
			var sr = new StreamReader(HttpContext.Current.Server.MapPath(templatePath));
			var template = sr.ReadToEnd();

            sr.Close();

			var mailer = new Mailer(template);
			mailer.BodyMimeType = Mailer.MimeType.HTML;
			mailer.UseAsyncSend = false;

			var fromEmail = new MailAddress(ConfigurationManager.AppSettings["RequestMeeting.FromAddress"], "Vores Carlsberg");
			var toEmail =
				HttpContext.Current.IsDebuggingEnabled
					? new MailAddress("mikael@noerd.dk")
					: new MailAddress(model.DealerEmail);
			var templateValues = new Dictionary<string, string>();
			var subject = ConfigurationManager.AppSettings["RequestMeeting.SubjectLine"];

			templateValues.Add("%STORENAME%", model.DealerName);
			templateValues.Add("%CLIENTNAME%", model.Name);
			templateValues.Add("%CLIENTPHONENO%", model.Phone);
			templateValues.Add("%CLIENTEMAIL%", model.Email);
			templateValues.Add("%CLIENTPOSTALCODE%", model.PostalCode);
			templateValues.Add("%CLIENTDESCRIPTION%", model.Description);

			mailer.SendMessage(fromEmail, toEmail, subject, templateValues);
			
			return true;
		}
	}
}
