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
	[EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net,http://books.maaninka.nu", "*", "*")]
	public class ReadersController : ApiController
	{
		private ReadersRepository repo;
		private DBContextBook context;
		public ReadersController()
		{
			context = new DBContextBook();
			repo = new ReadersRepository(context);

		}

		//[EnableQuery()]
		public IQueryable<ReaderModel> Get()
		{
			return repo.GetAllReaders().AsQueryable();
		}
	}
}
