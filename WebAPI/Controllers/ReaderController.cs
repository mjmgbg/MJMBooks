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
	public class ReaderController : ApiController
	{
		private ReadersRepository repo;
		private DBContextBook context;
		public ReaderController()
		{
			context = new DBContextBook();
			repo = new ReadersRepository(context);
		}

		[EnableQuery()]
		public IQueryable<ReaderModel> Get()
		{
			return repo.GetAllReaders().AsQueryable();
		}
		public ReaderModel Get(int id)
		{
			try
			{
				return repo.GetReaderById(id);

			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Post([FromBody]ReaderModel model)
		{
			repo.Add(model);
		}
		public void Put(int id, [FromBody]ReaderModel model)
		{
			repo.Edit(model);
		}

		public void Delete(int id)
		{
			repo.Delete(id);
		}
	}
}
