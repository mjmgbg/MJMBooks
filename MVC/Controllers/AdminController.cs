using Entities;
using MVC.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
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
				var savedInDb = await apiModelBook.IsBookInDb("api/DbBook?isbn=", model.Isbn);
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
				apiModelBook.SaveBookToDb("api/DbBook/", model.Book);
				return PartialView("_BookSaved");
			}

			return PartialView("_BookInfo", model);
		}

		public ActionResult Edit(int id)
		{
			if (id > 0)
			{
				var book = apiModelBook.GetBookFromDbById("api/APIDbBook/", id);
				return View("EditBook", book);
			}
			return View("EditBook");
		}


		public ActionResult EditBookList()
		{
			return null;

			//return View("ListBooks", CacheHelper.GetAllBooks("0", "0"));
		}

		[HttpPost]
		public async Task<ActionResult> Edit(int id, BookModel model)
		{
			try
			{
				//var book = await apiModelBook.UpdateBook("api/APIDbBook/", id.ToString(), model);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		public ActionResult Delete(int id)
		{
			var book = apiModelBook.GetBookFromDbById("api/APIDbBook/", id);
			return View("DeleteBook", book);
		}

		[HttpPost]
		public async Task<ActionResult> Delete(int id, BookModel model)
		{
			try
			{
				//await apiModelBook.DeleteBook("api/APIDbBook/", id.ToString(), model);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
		
	}
}