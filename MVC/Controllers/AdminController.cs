using Entities;
using MVC.Models;
using System;
using System.IO;
using System.Net.Http;
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

		public PartialViewResult Add()
		{
			return PartialView("_GetBookInfoFromISBN");
		}

		public PartialViewResult AddWithError(int error)
		{
			if (error == 1)
			{
				ViewBag.Error = "Du måste ange ISBN";
			}
			else
			{
				ViewBag.Error = "Något gick fel - försök igen.";
			}

			return PartialView("_GetBookInfoFromISBN");
		}

		public ActionResult EditBookList()
		{
			return null;

			//return View("ListBooks", CacheHelper.GetAllBooks("0", "0"));
		}


		[HttpPost]
		public ActionResult GetInfoFromAdlibris(string ISBN)
		{
			if (!string.IsNullOrWhiteSpace(ISBN))
			{
				var model = new BookViewModel();

				apiModelAdLibris.BookForAdLibris = apiModelAdLibris.GetBookFromAdlibris(ISBN, "api/AdLibris/");
				model.Book = apiModelAdLibris.BookForAdLibris;
				return PartialView("_BookInfo", model);
			}
			
			return RedirectToAction("AddWithError", new { error = 1 });
		}

		[HttpPost]
		public async Task<ActionResult> SaveBook(BookViewModel model)
		{
			if (ModelState.IsValid)
			{
				var savedInDb = await apiModelBook.IsBookInDb("api/DbBook?isbn=", model.Book.ISBN);
				model.Book.AlreadyInDb = savedInDb;

				if (model.Book.AlreadyInDb)
				{
					return View("_BookInfo", model);
				}

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