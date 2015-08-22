using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entities
{
	public class ReaderModel :IPerson
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[Display(Name = "Uppläsare:")]
		public string DisplayFullName { get { return FirstName + " " + LastName; } }
		public List<BookModel> Books { get; set; }
		public ReaderModel()
		{
			Books = new List<BookModel>();
		}
		
	}
}
