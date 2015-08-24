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
			model.BookList = apiModelBook.GetAllBooksFromDb("api/Book?");
			return View(model);
		}
		public ActionResult GetSeries(int id)
		{
			var model = new StartPageViewModel();
			model.BookList = apiModelBook.GetAllBooksFromDb("api/Book?$filter=SeriesId eq "+id+"&$orderby=SeriesPartId asc");
			return PartialView("Index",model);
		}
		public ActionResult GetAuthors(int id)
		{
			var model = new StartPageViewModel();
			model.BookList = apiModelBook.GetAllBooksFromDb("api/Book?");
			return PartialView("Index", model);
		}

		public ActionResult GetReaders(int id)
		{
			var model = new StartPageViewModel();
			model.BookList = apiModelBook.GetAllBooksFromDb("api/Book?");
			return PartialView("Index", model);
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