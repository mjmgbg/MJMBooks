using Entities;
using System.Collections.Generic;

namespace MVC.Models
{
	public class StartPageViewModel
	{
		public List<BookModel> BookList { get; set; }

		public bool NoSearchResultFound { get; set; }

		public bool ErrorInSearchResult { get; set; }
		public int TotalCount { get; set; }

		public StartPageViewModel()
		{
			BookList = new List<BookModel>();
		}

		public string GetReaders(BookModel book)
		{
			List<IPerson> list = new List<IPerson>(book.Readers);
			return ConvertNames(list);
		}
		public string GetAuthors(BookModel book)
		{
			List<IPerson> list = new List<IPerson>(book.Authors);
			return ConvertNames(list);
		}
		private string ConvertNames(List<IPerson> list)
		{
			var temp = string.Empty;
			foreach (var item in list)
			{
				if (list.Count == 1) { return item.DisplayFullName; }
				if (list.Count == 2) { temp += item.LastName + " & "; }
				if (list.Count > 2) { temp += item.LastName + ", "; }
			}
			if (list.Count == 2) { temp = temp.Substring(0, temp.Length - 3); }
			if (list.Count > 2) { temp = temp.Substring(0, temp.Length - 2); }

			return temp;
		}
	}
}