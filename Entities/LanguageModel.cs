using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entities
{
	public class LanguageModel
	{
		public int Id { get; set; }
		[Display(Name = "Språk:")]
		public string Name { get; set; }
	

	}
}

