using Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data
{
	public class SeriesRepository
	{
		private DBContextBook context;

		public SeriesRepository(DbContext dbContext)
		{
			context = dbContext as DBContextBook;
		}

		public List<SeriesModel> GetAllSeries()
		{
			return context.Series.ToList();
		}
	}
}