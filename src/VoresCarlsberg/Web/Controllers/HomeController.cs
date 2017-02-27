using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using VoresCarlsberg.Web.Models;

namespace VoresCarlsberg.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Edit(string id)
		{
			var viewModel = new SignupModel();

			//return View(viewModel);
			return View("Index", viewModel);
			//return View();
		}

    }
}