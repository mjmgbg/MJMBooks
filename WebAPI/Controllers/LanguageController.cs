using Business.DTO;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData;
using Business;
using Entities;

namespace WebAPI.Controllers
{
    [EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net,http://books.maaninka.nu", "*", "*")]
    public class LanguageController : ApiController
    {
        private readonly UnitOfWork _uow;

        public LanguageController()
        {
            _uow = new UnitOfWork();
        }

        [EnableQuery]
        public IQueryable<NameViewModel> Get()
        {
            return Converters.ConvertLookUpDbToModel(_uow.LanguageRepository.GetAll().ToList()).AsQueryable();
        }

        public NameViewModel Get(int id)
        {
            return Converters.ConvertLookUpDbToModel(_uow.LanguageRepository.GetById(id));
        }

        public void Post([FromBody] NameViewModel model)
        {
            var entity = Converters.ConvertLookUpModelToDb<LanguageModel>(model);
            _uow.LanguageRepository.InsertOrUpdate(entity);
            _uow.Commit();
        }

        public void Put(int id, [FromBody] NameViewModel model)
        {
            var entity = Converters.ConvertLookUpModelToDb<LanguageModel>(model);
            _uow.LanguageRepository.InsertOrUpdate(entity);
            _uow.Commit();
        }

        public void Delete(int id)
        {
            if (id > 0)
            {
                _uow.LanguageRepository.Delete(id);
                _uow.Commit();
            }
        }
    }
}