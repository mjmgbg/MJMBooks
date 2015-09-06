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
    public class PublisherController : ApiController
    {
        private readonly UnitOfWork _uow;

        public PublisherController()
        {
            _uow = new UnitOfWork();
        }

        [EnableQuery]
        public IQueryable<NameViewModel> Get()
        {
            return Converters.ConvertLookUpDbToModel(_uow.PublisherRepository.GetAll().ToList()).AsQueryable();
        }

        public NameViewModel Get(int id)
        {
            return Converters.ConvertLookUpDbToModel(_uow.PublisherRepository.GetById(id));
        }

        public void Post([FromBody] NameViewModel model)
        {
            var entity = Converters.ConvertLookUpModelToDb<PublisherModel>(model);
            _uow.PublisherRepository.InsertOrUpdate(entity);
            _uow.Commit();
        }

        public void Put(int id, [FromBody] NameViewModel model)
        {
            var entity = Converters.ConvertLookUpModelToDb<PublisherModel>(model);
            _uow.PublisherRepository.InsertOrUpdate(entity);
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