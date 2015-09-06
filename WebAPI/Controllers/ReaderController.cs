using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData;
using Business;
using Business.DTO;
using Data;
using Entities;

namespace WebAPI.Controllers
{
    [EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net,http://books.maaninka.nu", "*", "*")]
    public class ReaderController : ApiController
    {
        private readonly UnitOfWork _uow;

        public ReaderController()
        {
            _uow = new UnitOfWork();
        }

        [EnableQuery]
        public IQueryable<PersonViewModel> Get()
        {
            return Converters.ConvertPersonDbToModel(_uow.ReaderRepository.GetAll().ToList()).AsQueryable();
        }

        public PersonViewModel Get(int id)
        {
            return Converters.ConvertPersonDbToModel(_uow.ReaderRepository.GetById(id));
        }

        public void Post([FromBody] PersonViewModel model)
        {
            var entity = Converters.ConvertModelPersonToDb<ReaderModel>(model);
            _uow.ReaderRepository.InsertOrUpdate(entity);
            _uow.Commit();
        }

        public void Put(int id, [FromBody] PersonViewModel model)
        {
            var entity = Converters.ConvertModelPersonToDb<ReaderModel>(model);
            _uow.ReaderRepository.InsertOrUpdate(entity);
            _uow.Commit();
        }

        public void Delete(int id)
        {
            if (id > 0)
            {
                _uow.ReaderRepository.Delete(id);
                _uow.Commit();
            }
        }
    }
}