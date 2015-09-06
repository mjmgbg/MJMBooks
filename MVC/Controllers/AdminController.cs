using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Business;
using Business.DTO;

namespace MVC.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly GetApiResponse<BookDetailDto> _adlibrisApiModel;
        private readonly GetApiResponse<BookViewModel> _bookApiModel;
        private readonly GetApiResponse<NameViewModel> _nameApiModel;
        private readonly GetApiResponse<PersonViewModel> _personApiModel;


        public AdminController()
        {
            _adlibrisApiModel = new GetApiResponse<BookDetailDto>(ConfigurationManager.AppSettings["apiBaseUri"]);
            _bookApiModel = new GetApiResponse<BookViewModel>(ConfigurationManager.AppSettings["apiBaseUri"]);
            _nameApiModel = new GetApiResponse<NameViewModel>(ConfigurationManager.AppSettings["apiBaseUri"]);
            _personApiModel = new GetApiResponse<PersonViewModel>(ConfigurationManager.AppSettings["apiBaseUri"]);
        }

        public ActionResult Index()
        {
            return View();
        }

        private async Task<List<NameViewModel>> ListEntity(string url, GetApiResponse<NameViewModel> api)
        {
            return await api.GetAllNamesFromDb(url);
        }

        private async Task<List<PersonViewModel>> ListEntity(string url, GetApiResponse<PersonViewModel> api)
        {
            return await api.GetAllPersonsFromDb(url);
        }

        private async Task<ActionResult> ListEntity(string url, string view, GetApiResponse<NameViewModel> api)
        {
            var model = await api.GetAllNamesFromDb(url);
            return PartialView(view, model);
        }

        private async Task<ActionResult> ListEntity(string url, string view, GetApiResponse<PersonViewModel> api)
        {
            var model = await api.GetAllPersonsFromDb(url);
            return PartialView(view, model);
        }

        private async Task<ActionResult> SaveEntity(NameViewModel model, string url, GetApiResponse<NameViewModel> api)
        {
            if (ModelState.IsValid)
            {
                var result = await api.SaveNameToDb(url, model);
                if (result)
                {
                    return PartialView("_Saved");
                }
            }
            return PartialView("_Error");
        }

        private async Task<ActionResult> SaveEntity(PersonViewModel model, string url,
            GetApiResponse<PersonViewModel> api)
        {
            if (ModelState.IsValid)
            {
                var result = await api.SavePersonToDb(url, model);
                if (result)
                {
                    return PartialView("_Saved");
                }
            }
            return PartialView("_Error");
        }

        private async Task<ActionResult> EditEntity(int id, string url, string view, GetApiResponse<NameViewModel> api)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = await api.GetNameFromDbById(url, id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return PartialView(view, entity);
        }

        private async Task<ActionResult> EditEntity(int id, string url, string view, GetApiResponse<PersonViewModel> api)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = await api.GetPersonFromDbById(url, id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return PartialView(view, entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<ActionResult> EditEntity(int id, NameViewModel model, string url, string view,
            string errorView, GetApiResponse<NameViewModel> api)
        {
            var entity = await api.UpdateName(url, id.ToString(), model);
            if (ModelState.IsValid)
            {
                if (entity != null)
                {
                    return PartialView(view);
                }
            }
            return PartialView(errorView, entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<ActionResult> EditEntity(int id, PersonViewModel model, string url, string view,
            string errorView, GetApiResponse<PersonViewModel> api)
        {
            var entity = await api.UpdatePerson(url, id.ToString(), model);
            if (ModelState.IsValid)
            {
                if (entity != null)
                {
                    return PartialView(view);
                }
            }
            return PartialView(errorView, entity);
        }

        private async Task<ActionResult> DeleteEntity(int id, string url, string view, string errorView,
            GetApiResponse<NameViewModel> api)
        {
            var publisher = await api.GetNameFromDbById(url, id);
            if (publisher != null)
            {
                return PartialView(view, publisher);
            }
            return PartialView(errorView);
        }

        private async Task<ActionResult> DeleteEntity(int id, string url, string view, string errorView,
            GetApiResponse<PersonViewModel> api)
        {
            var publisher = await api.GetPersonFromDbById(url, id);
            if (publisher != null)
            {
                return PartialView(view, publisher);
            }
            return PartialView(errorView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<ActionResult> DeleteEntity(int id, NameViewModel model, string url, string view,
            string errorView, GetApiResponse<NameViewModel> api)
        {
            var result = await api.DeleteName(url, id.ToString());
            if (result)
            {
                return PartialView(view);
            }
            return PartialView(errorView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<ActionResult> DeleteEntity(int id, PersonViewModel model, string url, string view,
            string errorView, GetApiResponse<PersonViewModel> api)
        {
            var result = await api.DeletePerson(url, id.ToString());
            if (result)
            {
                return PartialView(view);
            }
            return PartialView(errorView);
        }

        private async Task GetDropDownLists(BookViewModel book)
        {
            var authorList = await ListEntity("api/Author?", _personApiModel);
            var readerList = await ListEntity("api/Reader?", _personApiModel);

            book.LanguageChoices = new SelectList(await ListEntity("api/Language?", _nameApiModel), "Id", "Name",
                book.LanguageId);
            book.PublisherChoices = new SelectList(await ListEntity("api/Publisher?", _nameApiModel), "Id", "Name",
                book.PublisherId);
            book.SeriesChoices = new SelectList(await ListEntity("api/Serie?", _nameApiModel), "Id", "Name",
                book.SeriesId);
            book.SeriesPartChoices = new SelectList(await ListEntity("api/SeriePart?", _nameApiModel), "Id", "Name",
                book.SeriesPartId);
            
            book.AuthorsList = book.Authors.Select(a => a.Id).ToArray();
            book.AuthorsChoices = new SelectList(authorList, "Id", "DisplayFullName", book.AuthorsList);

            book.ReadersList = book.Authors.Select(r => r.Id).ToArray();
            book.ReadersChoices = new SelectList(readerList, "Id", "DisplayFullName", book.ReadersList);


        }

        public async Task<ActionResult> PublisherList()
        {
            return await ListEntity("api/Publisher?", "_PublisherList", _nameApiModel);
        }

        public ActionResult AddPublisher()
        {
            return PartialView("_AddPublisher");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePublisher(NameViewModel model)
        {
            return await SaveEntity(model, "api/Publisher/", _nameApiModel);
        }

        public async Task<ActionResult> EditPublisher(int id)
        {
            return await EditEntity(id, "api/Publisher/", "_EditPublisher", _nameApiModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPublisher(int id, NameViewModel model)
        {
            return await EditEntity(id, model, "api/Publisher/", "Index", "_EditPublisher", _nameApiModel);
        }

        public async Task<ActionResult> DeletePublisher(int id)
        {
            return await DeleteEntity(id, "api/Publisher/", "_DeletePublisher", "_Error", _nameApiModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePublisher(int id, NameViewModel model)
        {
            return await DeleteEntity(id, model, "api/Publisher/", "Index", "_Error", _nameApiModel);
        }


        public async Task<ActionResult> SerieList()
        {
            return await ListEntity("api/Serie?", "_SerieList", _nameApiModel);
        }

        public ActionResult AddSerie()
        {
            return PartialView("_AddSerie");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveSerie(NameViewModel model)
        {
            return await SaveEntity(model, "api/Serie/", _nameApiModel);
        }

        public async Task<ActionResult> EditSerie(int id)
        {
            return await EditEntity(id, "api/Serie/", "_EditSerie", _nameApiModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSerie(int id, NameViewModel model)
        {
            return await EditEntity(id, model, "api/Serie/", "Index", "_EditSerie", _nameApiModel);
        }

        public async Task<ActionResult> DeleteSerie(int id)
        {
            return await DeleteEntity(id, "api/Serie/", "_DeleteSerie", "_Error", _nameApiModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteSerie(int id, NameViewModel model)
        {
            return await DeleteEntity(id, model, "api/Serie/", "Index", "_Error", _nameApiModel);
        }


        public async Task<ActionResult> ReaderList()
        {
            return await ListEntity("api/Reader?", "_ReaderList", _personApiModel);
        }

        public ActionResult AddReader()
        {
            return PartialView("_AddReader");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveReader(PersonViewModel model)
        {
            return await SaveEntity(model, "api/Reader/", _personApiModel);
        }

        public async Task<ActionResult> EditReader(int id)
        {
            return await EditEntity(id, "api/Reader/", "_EditReader", _personApiModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditReader(int id, PersonViewModel model)
        {
            return await EditEntity(id, model, "api/Reader/", "Index", "_EditReader", _personApiModel);
        }

        public async Task<ActionResult> DeleteReader(int id)
        {
            return await DeleteEntity(id, "api/Reader/", "_DeleteReader", "_Error", _personApiModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteReader(int id, PersonViewModel model)
        {
            return await DeleteEntity(id, model, "api/Reader/", "Index", "_Error", _personApiModel);
        }


        public async Task<ActionResult> AuthorList()
        {
            return await ListEntity("api/Author?", "_AuthorList", _personApiModel);
        }

        public ActionResult AddAuthor()
        {
            return PartialView("_AddAuthor");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAuthor(PersonViewModel model)
        {
            return await SaveEntity(model, "api/Author/", _personApiModel);
        }

        public async Task<ActionResult> EditAuthor(int id)
        {
            return await EditEntity(id, "api/Author/", "_EditAuthor", _personApiModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAuthor(int id, PersonViewModel model)
        {
            return await EditEntity(id, model, "api/Author/", "Index", "_EditAuthor", _personApiModel);
        }

        public async Task<ActionResult> DeleteAuthor(int id)
        {
            return await DeleteEntity(id, "api/Author/", "_DeleteAuthor", "_Error", _personApiModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAuthor(int id, PersonViewModel model)
        {
            return await DeleteEntity(id, model, "api/Author/", "Index", "_Error", _personApiModel);
        }

        public async Task<ActionResult> BookList()
        {
            var model = await _bookApiModel.GetAllBooksFromDb("api/Book?");
            return PartialView("_BookList", model);
        }

        public ActionResult AddBook()
        {
            return PartialView("_ISBN");
        }

        //TODO:Add book without info from adlibris
        public ActionResult CreateBook()
        {
           return null;
        }

        //Add book without info from adlibris
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBook(
            [Bind(
                Include =
                    "Id,Title,ImagePath,ISBN,LanguageId,PublishingDate,PublisherId,Description,BgColor,TextColor,TextColorSecond,SeriesId,SeriesPartId,CreateDate,UpdateDate,IsRead"
                )] BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookApiModel.SaveBookToDb("api/Book/", model);
                if (result)
                {
                    return PartialView("_Saved");
                }
            }
            return PartialView("_Error");
        }

        [HttpPost]
        public async Task<ActionResult> GetInfoFromAdlibris(IsbnViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_ISBN", model);
            var savedInDb = await _bookApiModel.IsBookInDb("api/Book?isbn=", model.Isbn);
            model.AlreadyInDb = savedInDb;

            if (model.AlreadyInDb)
            {
                return PartialView("_ISBN", model);
            }
            var bookModel = _adlibrisApiModel.GetBookFromAdlibris(model.Isbn, "api/AdLibris/");
            return PartialView("_BookInfo", bookModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveBook(BookDetailDto model)
        {
            if (!ModelState.IsValid) return PartialView("_BookInfo", model);
            var result = await _bookApiModel.SaveBookToDb("api/Book/", model);
            return PartialView(!result ? "_Error" : "_Saved");
        }

        public async Task<ActionResult> EditBook(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = await _bookApiModel.GetBookFromDbById("api/Book/", id);
            if (book == null)
            {
                return HttpNotFound();
            }

            await GetDropDownLists(book);
            return PartialView("_EditBook", book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBook(int id, BookViewModel model)
        {
            var book = await _bookApiModel.UpdateBook("api/Book/", id.ToString(), model);
            if (ModelState.IsValid)
            {
                if (book != null)
                {
                    return PartialView("Index");
                }
            }

            await GetDropDownLists(book);
            return PartialView("_EditBook", book);
            
        }

        public async Task<ActionResult> DeleteBook(int id)
        {
            var book = await _bookApiModel.GetBookFromDbById("api/Book/", id);
            if (book.Id > 0)
            {
                return PartialView("_DeleteBook", book);
            }
            return PartialView("_Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteBook(int id, BookViewModel model)
        {
            var result = await _bookApiModel.DeleteBook("api/Book/", id.ToString());
            if (result)
            {
                return PartialView("Index");
            }
            return PartialView("_Error");
        }

        //TODO: Needs Implementation
        public ActionResult EditSeriePart()
        {
            throw new NotImplementedException();
        }
    }
}