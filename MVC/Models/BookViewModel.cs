using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace MVC.Models
{
	public class BookViewModel
	{
		public FileUpload File { get; set; }
		public BookDetailDTO Book { get; set; }
	}
}
