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
	}
}