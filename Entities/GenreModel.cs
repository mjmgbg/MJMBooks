using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entities
{
	public class GenreModel 
	{
		public int Id { get; set; }
		[Display(Name = "Genre")]
		public string Name { get; set; }
		public List<BookModel> Books { get; set; }
		public GenreModel()
		{
			Books = new List<BookModel>();
		}
	
	}
}
