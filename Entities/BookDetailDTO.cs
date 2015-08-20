using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Entities
{
	public class BookDetailDTO
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Ange titel")]
		[DisplayName("Titel")]
		public string Title { get; set; }
		[Required(ErrorMessage = "Ange ISBN")]
		[DisplayName("ISBN")]
		public string ISBN { get; set; }
		[Required(ErrorMessage = "Beskriv handling")]
		[DisplayName("Handling")]
		public string Description { get; set; }
		[Required(ErrorMessage = "Ange sökväg till bild")]
		[DisplayName("Sökväg till bild")]
		public string ImagePath { get; set; }
		[Required(ErrorMessage = "Ange utgivningsdatum")]
		[DisplayName("Utgivningsdatum")]
		public string PublishingDate { get; set; }
		[Required(ErrorMessage = "Ange författare")]
		[DisplayName("Författare")]
		public string AuthorNames { get; set; }
		[DisplayName("Uppläsare")]
		public string ReaderNames { get; set; }
		[DisplayName("Bokserie")]
		public string SeriesName { get; set; }
		[Required(ErrorMessage = "Ange förlag")]
		[DisplayName("Förlag")]
		public string PublisherName { get; set; }
		[Required(ErrorMessage = "Ange språk")]
		[DisplayName("Språk")]
		public string Language { get; set; }
		[DisplayName("Genrer")]
		[DataType(DataType.MultilineText)]
		public string GenreNames { get; set; }
		public string BgColor { get; set; }
		public string TextColor { get; set; }
		public string TextColorSecond { get; set; }
		public DateTime CreateDate { get; set; }
		[DisplayName("Har läst boken")]
		public bool IsRead { get; set; }
		
	}
}
