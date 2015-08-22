using Entities;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
	public class HomeController : Controller
	{
		private GetApiResponse<BookModel> apiModelBook;
		public HomeController()
		{
			apiModelBook = new GetApiResponse<BookModel>();
		}
		public ActionResult Index()
		{
			var model = new StartPageViewModel();
			GetApiResponse<BookModel> apiModelBook = new GetApiResponse<BookModel>();
			model.BookList = apiModelBook.GetAllBooksFromDb("api/DbBook?");
			
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