using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
namespace Entities
{
	public class BookModel{
		 public BookModel()
		{
			this.Authors = new List<AuthorModel>();
			this.Genres = new List<GenreModel>();
			this.Readers = new List<ReaderModel>();
			
		}
	
		public int Id { get; set; }
		 [Display(Name = "Titel:")]
		public string Title { get; set; }
		 [Required(ErrorMessage = "Ange sökväg till bild")]
		 [DisplayName("Sökväg till bild")]
		 public string ImagePath { get; set; }
		 [Display(Name = "ISBN:")]
		public string ISBN { get; set; }
		public int LanguageId { get; set; }
		public int SeriesId { get; set; }
		public int SeriesPartId { get; set; }
		[UIHint("DateTimePicker")]
		[Display(Name = "Utgiven:")]
		public DateTime PublishingDate { get; set; }
		public int PublisherId { get; set; }		
		[Display(Name = "Handling:")]
		public string Description { get; set; }
		public LanguageModel Language { get; set; }
		public PublisherModel Publisher { get; set; }
		public List<AuthorModel> Authors { get; set; }
		public List<GenreModel> Genres { get; set; }
		public List<ReaderModel> Readers { get; set; }
		public SeriesModel Series { get; set; }
		public SeriesPartNumberModel SeriesPart { get; set; }
		public string BgColor { get; set; }
		public string TextColor { get; set; }
		public string TextColorSecond { get; set; }
		
	}
}
