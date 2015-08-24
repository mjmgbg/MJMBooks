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
	}
}