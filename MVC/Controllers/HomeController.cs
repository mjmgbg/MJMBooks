using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Business;
using Business.DTO;
using MVC.Models;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly GetApiResponse<BookViewModel> _apiModelBook;

        public HomeController()
        {
            _apiModelBook = new GetApiResponse<BookViewModel>(ConfigurationManager.AppSettings["apiBaseUri"]);
        }

        public async Task<ActionResult> Index()
        {
            var model = new StartPageViewModel {BookList = await _apiModelBook.GetAllBooksFromDb("api/Book?")};
            model.TotalCount = model.BookList.Count;
            return View(model);
        }

        public async Task<ActionResult> GetSeries(int id)
        {
            var model = new StartPageViewModel
            {
                BookList = await
                    _apiModelBook.GetAllBooksFromDb("api/Book?$filter=SeriesId eq " + id + "&$orderby=SeriesPartId asc")
            };
            model.TotalCount = model.BookList.Count;
            return PartialView("Index", model);

        }

        public async Task<ActionResult> GetAuthors(int id)
        {
            var model = new StartPageViewModel
            {
                BookList = await
                    _apiModelBook.GetAllBooksFromDb("api/Book?$filter=Authors/any(a: a/Id eq " + id +
                                                    ")&$orderby=Title asc")
            };
            model.TotalCount = model.BookList.Count;
            return PartialView("Index", model);
        }

        public async Task<ActionResult> GetReaders(int id)
        {
            var model = new StartPageViewModel
            {
                BookList = await
                    _apiModelBook.GetAllBooksFromDb("api/Book?$filter=Readers/any(r: r/Id eq " + id +
                                                    ")&$orderby=Title asc")
            };
            model.TotalCount = model.BookList.Count;
            return PartialView("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> Search(string search)
        {
            string searchString;
            search = search.ToLower();

            if (search.IndexOf("&", StringComparison.Ordinal) > 0)
            {
                var tempGenreName = string.Empty;
                var tempAuthorFirstName = string.Empty;
                var tempAuthorLastName = string.Empty;
                var tempReaderFirstName = string.Empty;
                var tempReaderLastName = string.Empty;
                var stringArr = search.Split('&');

                foreach (var item in stringArr)
                {
                    tempGenreName += " contains(tolower(g/Name),  '" + item + "') and ";
                    tempAuthorFirstName += " contains(tolower(a/FirstName),  '" + item + "') and ";
                    tempAuthorLastName += " contains(tolower(a/LastName),  '" + item + "') and ";
                    tempReaderFirstName += " contains(tolower(r/FirstName),  '" + item + "') and ";
                    tempReaderLastName += " contains(tolower(r/LastName),  '" + item + "') and ";
                }
                tempGenreName = tempGenreName.Substring(0, tempGenreName.Length - 5);
                tempAuthorFirstName = tempAuthorFirstName.Substring(0, tempAuthorFirstName.Length - 5);
                tempAuthorLastName = tempAuthorLastName.Substring(0, tempAuthorLastName.Length - 5);
                tempReaderFirstName = tempReaderFirstName.Substring(0, tempReaderFirstName.Length - 5);
                tempReaderLastName = tempReaderLastName.Substring(0, tempReaderLastName.Length - 5);
                search = search.Replace("&", "");

                var startString = "api/Book?$filter=contains(tolower(Title), '" + search + "') " +
                                  "or ISBN eq '" + search + "' " +
                                  "or tolower(Language/Name) eq  '" + search + "' or Genres/any(g:";

                var reader = " or Readers/any(r:";
                var author = " or Authors/any(a:";

                searchString = startString + tempGenreName + ")" + reader + tempReaderFirstName + " and " +
                               tempReaderLastName + ")" + author + tempAuthorFirstName + " and " + tempAuthorLastName +
                               ")";
            }
            else
            {
                searchString = "api/Book?$filter=contains(tolower(Title), '" + search + "') " +
                               " or ISBN eq '" + search + "' " +
                               " or Authors/any(a1: contains(tolower(a1/FirstName),  '" + search + "')) " +
                               "or Authors/any(a2: contains(tolower(a2/LastName), '" + search +
                               "')) or Readers/any(r1: contains(tolower(r1/FirstName),  '" + search + "')) " +
                               "or Readers/any(r2: contains(tolower(r2/LastName), '" + search +
                               "')) or tolower(Language/Name) eq  '" + search + "'";
            }

            var model = new StartPageViewModel();
            var searchResults = await _apiModelBook.GetAllBooksFromDb(searchString);
            model.BookList = searchResults;

            if (model.BookList == null)
            {
                model.ErrorInSearchResult = true;
                return PartialView("Index", model);
            }
            if (model.BookList.Count == 0)
            {
                model.NoSearchResultFound = true;
            }
            return PartialView("Index", model);
        }
    }
}