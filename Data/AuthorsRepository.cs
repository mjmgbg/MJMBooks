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
			
		public AuthorModel GetAuthorById(int id)
		{
			return context.Authors.Where(s => s.Id == id).AsEnumerable().Select(a => new AuthorModel
				{
					Id = a.Id,
					FirstName = a.FirstName,
					LastName = a.LastName
				}).FirstOrDefault();
		}
		public void Delete(int id)
		{
			var authorToDelete = context.Authors.Find(id);
			context.Authors.Remove(authorToDelete);
			context.SaveChanges();
		}

		public void Edit(AuthorModel model)
		{
			var authorToEdit = context.Authors.Find(model.Id);
			authorToEdit.FirstName = model.FirstName;
			authorToEdit.LastName = model.LastName;
			context.SaveChanges();
		}
		public void Add(AuthorModel model)
		{
			var author = context.Authors.Where(s => s.FirstName.ToLower() == model.FirstName.ToLower() && s.LastName.ToLower() == model.LastName.ToLower());
			if (author != null)
			{
				AuthorModel newAuthor = new AuthorModel
				{
					FirstName = model.FirstName,
					LastName = model.LastName
				};
				context.Authors.Add(newAuthor);
				context.SaveChanges();
			}
		}

	}
}