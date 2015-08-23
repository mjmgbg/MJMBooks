using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPI.Controllers
{
	[EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net", "*", "*")]
	public class SeriesController : ApiController
	{
		private SeriesRepository repo;
		private DBContextBook context;
		public SeriesController()
		{
			context = new DBContextBook();
			repo = new SeriesRepository(context);

		}

		//[EnableQuery()]
		public IQueryable<SeriesModel> Get()
		{
			return repo.GetAllSeries().AsQueryable();
		}
	}
}
