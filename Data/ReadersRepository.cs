using Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data
{
	public class ReadersRepository
	{
		private DBContextBook context;

		public ReadersRepository(DbContext dbContext)
		{
			context = dbContext as DBContextBook;
		}

		public List<ReaderModel> GetAllReaders()
		{
			return context.Readers.ToList();
		}

		public ReaderModel GetReaderById(int id)
		{
			return context.Readers.Where(s => s.Id == id).AsEnumerable().Select(r => new ReaderModel
			{
				Id = r.Id,
				FirstName = r.FirstName,
				LastName = r.LastName
			}).FirstOrDefault();
		}
		public void Add(ReaderModel model)
		{
			var reader = context.Readers.Where(s => s.FirstName.ToLower() == model.FirstName.ToLower() && s.LastName.ToLower() == model.LastName.ToLower());
			if (reader != null)
			{
				ReaderModel newReader = new ReaderModel
				{
					FirstName=model.FirstName,
					LastName=model.LastName
				};
				context.Readers.Add(newReader);
				context.SaveChanges();
			}
		}
		
		public void Delete(int id)
		{
			var readerToDelete = context.Readers.Find(id);
			context.Readers.Remove(readerToDelete);
			context.SaveChanges();
		}

		public void Edit(ReaderModel model)
		{
			var readerToEdit = context.Readers.Find(model.Id);
			readerToEdit.FirstName = model.FirstName;
			readerToEdit.LastName = model.LastName;
			context.SaveChanges();
		}

	
	}
}