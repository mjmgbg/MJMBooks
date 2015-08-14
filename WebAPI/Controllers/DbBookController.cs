﻿using Data;
using Entities;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	[EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net","*","*"  )]
	public class DbBookController : ApiController
	{
		private BookRepository repo;
		private DBContextBook context;

		public DbBookController()
		{
			context = new DBContextBook();
			repo = new BookRepository(context);
		}

		//[EnableQuery()]
		public IQueryable<BookModel> Get()
		{
			return repo.GetAllBooks().AsQueryable();
		}

		public bool Get(string isbn)
		{
			return repo.IsBookInDB(isbn);
		}

		public BookModel Get(int id)
		{
			try
			{
				var book = repo.GetBookById(id);
				return book;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void Post([FromBody]BookDetailDTO model)
		{
			repo.Add(model);
		}

		public void Put(int id, [FromBody]BookModel book)
		{
			repo.Edit(book);
		}

		public void Delete(int id)
		{
			repo.Delete(id);
		}
	}
}