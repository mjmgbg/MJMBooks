using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
namespace Entities
{
	public class BookModel{
		public int Id { get; set; }
		 [Display(Name = "Titel")]
		public string Title { get; set; }
		 [Required(ErrorMessage = "Ange sökväg till bild")]
		 [DisplayName("Sökväg till bild")]
		 public string ImagePath { get; set; }
		 [Display(Name = "ISBN")]
		public string ISBN { get; set; }
		public int LanguageId { get; set; }
		public int ?SeriesId { get; set; }
		public int ?SeriesPartId { get; set; }
		[UIHint("DateTimePicker")]
		[Display(Name = "Utgiven")]
		public DateTime PublishingDate { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }
		public int PublisherId { get; set; }		
		[Display(Name = "Handling")]
		public string Description { get; set; }
		public LanguageModel Language { get; set; }
		public PublisherModel Publisher { get; set; }
		[Display(Name = "Författare")]
		public List<AuthorModel> Authors { get; set; }
		public List<GenreModel> Genres { get; set; }
		[Display(Name = "Uppläsare")]
		public List<ReaderModel> Readers { get; set; }
		public SeriesModel Series { get; set; }
		public SeriesPartNumberModel SeriesPart { get; set; }
		public string BgColor { get; set; }
		public string TextColor { get; set; }
		public string TextColorSecond { get; set; }
		public bool IsRead { get; set; }
		public BookModel()
		{
			Authors = new List<AuthorModel>();
			Genres = new List<GenreModel>();
			Readers = new List<ReaderModel>();
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
		public string GetReaders(BookModel book)
		{
			List<IPerson> list = new List<IPerson>(book.Readers);
			return book.ConvertNames(list);
		}
		public string GetAuthors(BookModel book)
		{
			List<IPerson> list = new List<IPerson>(book.Authors);
			return book.ConvertNames(list);
		}
	}
}
