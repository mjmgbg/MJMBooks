using Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data
{
	public class AuthorsRepository
	{
		private DBContextBook context;

		public AuthorsRepository(DbContext dbContext)
		{
			context = dbContext as DBContextBook;
		}

		public List<AuthorModel> GetAllAuthors()
		{
			return context.Authors.ToList();
		}
	}
}