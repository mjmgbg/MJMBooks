using Data;
using Entities;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData;
using WebAPI.Models;


namespace WebAPI.Controllers
{
	[EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net, http://books.maaninka.nu", "*", "*")]
	public class BookController : ApiController
	{
		private BookRepository repo;
		private DBContextBook context;

		public BookController()
		{
			context = new DBContextBook();
			repo = new BookRepository(context);
		}

		[EnableQuery()]
		public IQueryable<BookModel> Get()
		{
			return repo.GetAllBooks().AsQueryable();
		}

		public bool Get(string isbn)
		{
			return repo.IsBookInDB(isbn);
		}

		public BookModel Get(int id)
		{
			try
			{
				var book = repo.GetBookById(id);
				return book;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void Post([FromBody]BookDetailDTO model)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["storageConnection"].ConnectionString;
			string path = ConfigurationManager.AppSettings["storagePath"];
			repo.Add(connectionString, path, model);
		}

		public void Put(int id, [FromBody]BookModel book)
		{
			repo.Edit(book);
		}

		public void Delete(int id)
		{
			repo.Delete(id);
		}
	}
}