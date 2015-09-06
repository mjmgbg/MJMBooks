using System.Collections.Generic;

namespace Business.DTO
{
    public class StartPageViewModel
    {
        public List<BookViewModel> BookList { get; set; }

        public bool NoSearchResultFound { get; set; }

        public bool ErrorInSearchResult { get; set; }
        public int TotalCount { get; set; }

        public StartPageViewModel()
        {
            BookList = new List<BookViewModel>();
        }
    }
}