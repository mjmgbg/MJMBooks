using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData;

namespace WebAPI.Controllers
{
	[EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net,http://books.maaninka.nu", "*", "*")]
	public class SerieController : ApiController
	{
		private SeriesRepository repo;
		private DBContextBook context;
		public SerieController()
		{
			context = new DBContextBook();
			repo = new SeriesRepository(context);
		}

		[EnableQuery()]
		public IQueryable<SeriesModel> Get()
		{
			return repo.GetAllSeries().AsQueryable();
		}
		public SeriesModel Get(int id)
		{
			try
			{
				return repo.GetSerieById(id);
				
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void Post([FromBody]SeriesModel model)
		{			
			repo.Add(model);
		}
		public void Put(int id, [FromBody]SeriesModel model)
		{
			repo.Edit(model);
		}

		public void Delete(int id)
		{
			repo.Delete(id);
		}
	}
}
