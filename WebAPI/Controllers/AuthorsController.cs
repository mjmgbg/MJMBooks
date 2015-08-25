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
	[EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net, http://books.maaninka.nu", "*", "*")]
	public class AuthorsController : ApiController
	{
		private AuthorsRepository repo;
		private DBContextBook context;
		public AuthorsController()
		{
			context = new DBContextBook();
			repo = new AuthorsRepository(context);

		}

		//[EnableQuery()]
		public IQueryable<AuthorModel> Get()
		{
			return repo.GetAllAuthors().AsQueryable();
		}
	}
}
