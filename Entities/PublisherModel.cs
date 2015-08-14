using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entities
{
	public class PublisherModel : IDBLookup
	{
		public int Id { get; set; }
		[Display(Name = "Förlag:")]
		public string Name { get; set; }
			
	}
}
