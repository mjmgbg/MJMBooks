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
	[EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net, http://books.maaninka.nu", "*", "*")]
	public class AuthorController : ApiController
	{
		private AuthorsRepository repo;
		private DBContextBook context;
		public AuthorController()
		{
			context = new DBContextBook();
			repo = new AuthorsRepository(context);

		}

		[EnableQuery()]
		public IQueryable<AuthorModel> Get()
		{
			return repo.GetAllAuthors().AsQueryable();
		}
		public AuthorModel Get(int id)
		{
			try
			{
				return repo.GetAuthorById(id);

			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Post([FromBody]AuthorModel model)
		{
			repo.Add(model);
		}
		public void Put(int id, [FromBody]AuthorModel model)
		{
			repo.Edit(model);
		}

		public void Delete(int id)
		{
			repo.Delete(id);
		}
	}
}
