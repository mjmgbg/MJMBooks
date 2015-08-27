using Entities;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVC.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private GetApiResponse<BookDetailDTO> adlibrisApiModel;
		private GetApiResponse<BookModel> booksApiModel;
		private GetApiResponse<SeriesModel> seriesApiModel;
		private GetApiResponse<AuthorModel> authorsApiModel;
		private GetApiResponse<ReaderModel> readersApiModel;

		public AdminController()
		{
			adlibrisApiModel = new GetApiResponse<BookDetailDTO>();
			booksApiModel = new GetApiResponse<BookModel>();
			seriesApiModel = new GetApiResponse<SeriesModel>();
			authorsApiModel = new GetApiResponse<AuthorModel>();
			readersApiModel = new GetApiResponse<ReaderModel>();
		}

		public ActionResult Index()
		{
			return View();
		}
		#region "Serie"
		public ActionResult SerieList()
		{
			var model = new List<SeriesModel>();
			model = seriesApiModel.GetAllSeriesFromDb("api/Serie?");
			return PartialView("_SerieList", model);
		}

		public ActionResult AddSerie()
		{
			return PartialView("_AddSerie");
		}

		[HttpPost]
		public async Task<ActionResult> SaveSerie(SeriesModel serie)
		{
			if (ModelState.IsValid)
			{
				var result = await seriesApiModel.SaveSerieToDb("api/Serie/", serie);
				if (result)
				{
					return PartialView("_Saved");
				}
			}
			return PartialView("_Error");
		}
		public ActionResult EditSerie(int id)
		{
			if (id > 0)
			{
				var serie = seriesApiModel.GetSerieFromDbById("api/Serie/", id);
				if (serie!=null)
				{
					return PartialView("_EditSerie", serie);
				}
			}
			return PartialView("_Error");
		}

		[HttpPost]
		public async Task<ActionResult> EditSerie(int id, SeriesModel model)
		{
			await seriesApiModel.UpdateSerie("api/Serie/", id.ToString(), model);
			return PartialView("Index");
			
		}
		public ActionResult DeleteSerie(int id)
		{
			var serie = seriesApiModel.GetSerieFromDbById("api/Serie/", id);
			if (serie!=null)
			{
				return PartialView("_DeleteSerie", serie);
			}
			return PartialView("_Error");
		}

		[HttpPost]
		public async Task<ActionResult> DeleteSerie(int id, SeriesModel model)
		{
			var result = await seriesApiModel.DeleteSerie("api/Serie/", id.ToString(), model);
			if (result)
			{
				return PartialView("Index");
			}
			return PartialView("_Error");
		}
		#endregion

		#region "Reader"
		public ActionResult ReaderList()
		{
			var model = new List<ReaderModel>();
			model = readersApiModel.GetAllReadersFromDb("api/Reader?");
			return PartialView("_ReaderList", model);
		}

		public ActionResult AddReader()
		{
			return PartialView("_AddReader");
		}

		[HttpPost]
		public async Task<ActionResult> SaveReader(ReaderModel reader)
		{
			if (ModelState.IsValid)
			{
				var result = await readersApiModel.SaveReaderToDb("api/Reader/", reader);
				if (result)
				{
					return PartialView("_Saved");
				}
			}
			return PartialView("_Error");
		}
		public ActionResult EditReader(int id)
		{
			if (id > 0)
			{
				var reader = readersApiModel.GetReaderFromDbById("api/Reader/", id);
				if (reader != null)
				{
					return PartialView("_EditReader", reader);
				}
			}
			return PartialView("_Error");
		}

		[HttpPost]
		public async Task<ActionResult> EditReader(int id, ReaderModel model)
		{
			await readersApiModel.UpdateReader("api/Reader/", id.ToString(), model);
			return PartialView("Index");

		}
		public ActionResult DeleteReader(int id)
		{
			var reader = readersApiModel.GetReaderFromDbById("api/Reader/", id);
			if (reader != null)
			{
				return PartialView("_DeleteReader", reader);
			}
			return PartialView("_Error");
		}

		[HttpPost]
		public async Task<ActionResult> DeleteReader(int id, ReaderModel model)
		{
			var result = await readersApiModel.DeleteReader("api/Reader/", id.ToString(), model);
			if (result)
			{
				return PartialView("Index");
			}
			return PartialView("_Error");
		}
		#endregion

		#region "Author"
		public ActionResult AuthorList()
		{
			var model = new List<AuthorModel>();
			model = authorsApiModel.GetAllAuthorsFromDb("api/Author?");
			return PartialView("_AuthorList", model);
		}

		public ActionResult AddAuthor()
		{
			return PartialView("_AddAuthor");
		}

		[HttpPost]
		public async Task<ActionResult> SaveAuthor(AuthorModel author)
		{
			if (ModelState.IsValid)
			{
				var result = await authorsApiModel.SaveAuthorToDb("api/Author/", author);
				if (result)
				{
					return PartialView("_Saved");
				}
			}
			return PartialView("_Error");
		}
		public ActionResult EditAuthor(int id)
		{
			if (id > 0)
			{
				var author = authorsApiModel.GetAuthorFromDbById("api/Author/", id);
				if (author != null)
				{
					return PartialView("_EditAuthor", author);
				}
			}
			return PartialView("_Error");
		}

		[HttpPost]
		public async Task<ActionResult> EditAuthor(int id, AuthorModel model)
		{
			await authorsApiModel.UpdateAuthor("api/Author/", id.ToString(), model);
			return PartialView("Index");

		}
		public ActionResult DeleteAuthor(int id)
		{
			var author = authorsApiModel.GetAuthorFromDbById("api/Author/", id);
			if (author != null)
			{
				return PartialView("_DeleteAuthor", author);
			}
			return PartialView("_Error");
		}

		[HttpPost]
		public async Task<ActionResult> DeleteAuthor(int id, AuthorModel model)
		{
			var result = await authorsApiModel.DeleteAuthor("api/Author/", id.ToString(), model);
			if (result)
			{
				return PartialView("Index");
			}
			return PartialView("_Error");
		}
		#endregion
		
		#region "Book"
		public ActionResult BookList()
		{
			var model = new List<BookModel>();
			model = booksApiModel.GetAllBooksFromDb("api/Book?");
			return PartialView("_BookList", model);
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
				var savedInDb = await booksApiModel.IsBookInDb("api/Book?isbn=", model.Isbn);
				model.AlreadyInDb = savedInDb;

				if (model.AlreadyInDb)
				{
					return PartialView("_ISBN", model);
				}
				var bookModel = new BookViewModel();

				adlibrisApiModel.BookForAdLibris = adlibrisApiModel.GetBookFromAdlibris(model.Isbn, "api/AdLibris/");
				bookModel.Book = adlibrisApiModel.BookForAdLibris;
				return PartialView("_BookInfo", bookModel);
			}

			return PartialView("_ISBN", model);
		}

		[HttpPost]
		public async Task<ActionResult> SaveBook(BookViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await booksApiModel.SaveBookToDb("api/Book/", model.Book);
				if (result)
				{
					return PartialView("_BookSaved");
				}
			}
			return PartialView("_Error");
		}

		public ActionResult EditBook(int id)
		{
			if (id > 0)
			{
				var book = booksApiModel.GetBookFromDbById("api/Book/", id);
				if (book.Id > 0)
				{
					return PartialView("_EditBook", book);
				}
			}
			return PartialView("_Error");
		}

		[HttpPost]
		public async Task<ActionResult> EditBook(int id, BookModel model)
		{
			var resultView = await booksApiModel.UpdateBook("api/Book/", id.ToString(), model);
			if (resultView.Book.Id > 0)
			{
				return PartialView("Index");
			}
			return PartialView("_Error");
		}

		public ActionResult DeleteBook(int id)
		{
			var book = booksApiModel.GetBookFromDbById("api/Book/", id);
			if (book.Id > 0)
			{
				return PartialView("_DeleteBook", book);
			}
			return PartialView("_Error");
		}

		[HttpPost]
		public async Task<ActionResult> DeleteBook(int id, BookModel model)
		{
			var result = await booksApiModel.DeleteBook("api/Book/", id.ToString(), model);
			if (result)
			{
				return PartialView("Index");
			}
			return PartialView("_Error");
		}
		#endregion
	}
}