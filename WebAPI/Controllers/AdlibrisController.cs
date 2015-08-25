using Entities;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	[EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net,http://books.maaninka.nu", "*", "*")]
	public class AdlibrisController : ApiController
	{
		public void Get()
		{
		}

		public BookDetailDTO Get(string id)
		{
			var newBook = new BookDetailDTO();
			bool isLoaded = ParseHTML.LoadDocument(id);
			if (isLoaded)
			{
				newBook.Title = ParseHTML.GetTitle();
				newBook.AuthorNames = ParseHTML.GetAuthors();
				newBook.PublishingDate = ParseHTML.GetDate();
				newBook.ISBN = id;
				newBook.Description = ParseHTML.GetDescription();
				newBook.SeriesName = ParseHTML.GetSeries();
				newBook.PublisherName = ParseHTML.GetPublisher();
				newBook.Language = ParseHTML.GetLanguage();
				newBook.ReaderNames = ParseHTML.GetReader();
				newBook.GenreNames = ParseHTML.GetGenres();
			}

			return newBook;
		}
	}
}