using Entities;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private GetApiResponse<BookDetailDTO> apiModelAdLibris;
		private GetApiResponse<BookModel> apiModelBook;
		
	

		public AdminController()
		{
			apiModelAdLibris = new GetApiResponse<BookDetailDTO>();
			apiModelBook = new GetApiResponse<BookModel>();
		}

		public ActionResult Index()
		{
			return View();
		}
		public ActionResult BookList()
		{
			var model = new List<BookModel>();
			model = apiModelBook.GetAllBooksFromDb("api/Book?");
			return PartialView("_BookList", model);
		}

		public ActionResult Add()
		{
			return PartialView("_ISBN");
		}

		[HttpPost]
		public async Task<ActionResult> GetInfoFromAdlibris(ISBNViewModel model)
		{

			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (ModelState.IsValid)
			{
				var savedInDb = await apiModelBook.IsBookInDb("api/Book?isbn=", model.Isbn);
				model.AlreadyInDb = savedInDb;

				if (model.AlreadyInDb)
				{
					return PartialView("_ISBN", model);
				}
				var bookModel = new BookViewModel();

				apiModelAdLibris.BookForAdLibris = apiModelAdLibris.GetBookFromAdlibris(model.Isbn, "api/AdLibris/");
				bookModel.Book = apiModelAdLibris.BookForAdLibris;
				return PartialView("_BookInfo", bookModel);
			}
			
			return PartialView("_ISBN", model);
		}

		[HttpPost]
		public async Task<ActionResult> SaveBook(BookViewModel model)
		{
			if (ModelState.IsValid)
			{
				apiModelBook.SaveBookToDb("api/Book/", model.Book);
				return PartialView("_BookSaved");
			}

			return PartialView("_BookInfo", model);
		}

		public ActionResult EditBook(int id)
		{
			if (id > 0)
			{
				var book = apiModelBook.GetBookFromDbById("api/Book/", id);
				return PartialView("_EditBook", book);
			}
			return PartialView("_EditBook");
		}

		[HttpPost]
		public async Task<ActionResult> EditBook(int id, BookModel model)
		{
			try
			{
				var book = await apiModelBook.UpdateBook("api/Book/", id.ToString(), model);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		public ActionResult DeleteBook(int id)
		{
			var book = apiModelBook.GetBookFromDbById("api/Book/", id);
			return PartialView("_DeleteBook", book);
		}

		[HttpPost]
		public async Task<ActionResult> DeleteBook(int id, BookModel model)
		{
			try
			{
				await apiModelBook.DeleteBook("api/Book/", id.ToString(), model);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
		
	}
}