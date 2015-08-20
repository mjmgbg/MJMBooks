using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVC.Models
{
	public class ISBNViewModel
	{
		[Required(ErrorMessage = "Ange ISBN")]
		[MinLength(10,ErrorMessage = "Minst 10 tecken")]
		[DisplayName("ISBN")]
		public string Isbn { get; set; }
		public bool AlreadyInDb { get; set; }

		public ISBNViewModel()
		{
			AlreadyInDb = false;
		}
	}
}