using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData;
using Business.DTO;
using Data;

namespace WebAPI.Controllers
{
    [EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net, http://books.maaninka.nu", "*", "*")]
    public class BookController : ApiController
    {
        private readonly UnitOfWork _uow;

        public BookController()
        {
            _uow = new UnitOfWork();
        }

        [EnableQuery]
        public IQueryable<BookViewModel> Get()
        {
            return _uow.BookRepository.ConvertDbToModel(_uow.BookRepository.GetAll().ToList()).AsQueryable();
        }

        public bool Get(string isbn)
        {
            return _uow.BookRepository.IsBookInDb(isbn);
        }

        public BookViewModel Get(int id)
        {
            return _uow.BookRepository.ConvertDbToModel(_uow.BookRepository.GetById(id));
        }

        public void Post([FromBody] BookDetailDto model)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["storageConnection"].ConnectionString;
            _uow.BookRepository.AddFromAdlibris(connectionString,model);
            _uow.Commit();
        }

        public void Put(int id, [FromBody] BookViewModel book)
        {
            var dbBook = _uow.BookRepository.ConvertModelToDb(book);
            _uow.BookRepository.InsertOrUpdate(dbBook);
            _uow.Commit();
        }

        public void Delete(int id)
        {
            if (id > 0)
            {
                _uow.BookRepository.Delete(id);
                _uow.Commit();
            }
        }
    }
}