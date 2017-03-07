using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using VoresCarlsberg.Web.Models;

namespace VoresCarlsberg.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			//var url = Request.Headers["HOST"];
			//if (url.Contains("vorescarlsberg.nu") && !url.Contains("unoeuro"))
			//{
			//	return null;
			//}
			
			return View();
		}

		public ActionResult Edit(string id)
		{
			var viewModel = new LoginModel();

			//return View(viewModel);
			return View("Index", viewModel);
			//return View();
		}

    }
}