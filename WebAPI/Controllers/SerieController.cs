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
    public class SerieController : ApiController
    {
        private readonly UnitOfWork _uow;

        public SerieController()
        {
            _uow = new UnitOfWork();
        }

        [EnableQuery]
        public IQueryable<NameViewModel> Get()
        {
            return Converters.ConvertLookUpDbToModel(_uow.SerieRepository.GetAll().ToList()).AsQueryable();
        }

        public NameViewModel Get(int id)
        {
            return Converters.ConvertLookUpDbToModel(_uow.SerieRepository.GetById(id));
        }

        public void Post([FromBody] NameViewModel model)
        {
            var entity = Converters.ConvertLookUpModelToDb<SeriesModel>(model);
            _uow.SerieRepository.InsertOrUpdate(entity);
            _uow.Commit();
        }

        public void Put(int id, [FromBody] NameViewModel model)
        {
            var entity = Converters.ConvertLookUpModelToDb<SeriesModel>(model);
            _uow.SerieRepository.InsertOrUpdate(entity);
            _uow.Commit();
        }

        public void Delete(int id)
        {
            if (id > 0)
            {
                _uow.SeriePartRepository.Delete(id);
                _uow.Commit();
            }
        }
    }
}