using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace myoungblog.Controllers
{
	[RequireHttps]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Contact(Models.EmailModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var body = "<p>Email From: <bold>{0}</bold>({1})</p><p>Message:</p><p>{2}</p>";
					var from = "MyPortfolio<martingyoung22@gmail.com>"; 

					var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"]) { Subject = "Portfolio Contact Email",
						Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
						IsBodyHtml = true };

					var svc = new Models.EmailHelper(); await svc.SendAsync(email);

					return RedirectToAction("Index");
				}
				catch (Exception ex) { Console.WriteLine(ex.Message); await Task.FromResult(0); }
			}
			return View(model);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}