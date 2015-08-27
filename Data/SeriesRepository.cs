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

		public void Add(SeriesModel model)
		{
			var serie = context.Series.Where(s => s.Name.ToLower() == model.Name.ToLower());
			if (serie != null)
			{
				SeriesModel newSeries = new SeriesModel
				{
					Name = model.Name
				};
				context.Series.Add(newSeries);
				context.SaveChanges();
			}
		}

		public void Delete(int id)
		{
			var serieToDelete = context.Series.Find(id);
			context.Series.Remove(serieToDelete);
			context.SaveChanges();
		}

		public void Edit(SeriesModel model)
		{
			var serieToEdit = context.Series.Find(model.Id);
			serieToEdit.Name = model.Name;
			context.SaveChanges();
		}

	
		public SeriesModel GetSerieById(int id)
		{
			return context.Series.Where(s => s.Id == id).AsEnumerable().Select(s => new SeriesModel
			{
				Id = s.Id,
				Name = s.Name
			}).FirstOrDefault();
		}
	}
}