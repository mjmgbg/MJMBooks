using System.Web.Http;
using System.Web.Http.Cors;
using Business.DTO;
using Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net,http://books.maaninka.nu", "*", "*")]
    public class AdlibrisController : ApiController
    {
        public void Get()
        {
        }

        public BookDetailDto Get(string id)
        {
            var newBook = new BookDetailDto();
            var isLoaded = ParseHtml.LoadDocument(id);
            if (isLoaded)
            {
                newBook.Title = ParseHtml.GetTitle();
                newBook.AuthorNames = ParseHtml.GetAuthors();
                newBook.PublishingDate = ParseHtml.GetDate();
                newBook.Isbn = id;
                newBook.Description = ParseHtml.GetDescription();
                newBook.SeriesName = ParseHtml.GetSeries();
                newBook.PublisherName = ParseHtml.GetPublisher();
                newBook.Language = ParseHtml.GetLanguage();
                newBook.ReaderNames = ParseHtml.GetReader();
                newBook.GenreNames = ParseHtml.GetGenres();
            }

            return newBook;
        }
    }
}