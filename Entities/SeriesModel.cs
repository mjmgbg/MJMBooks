using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
	public class SeriesModel
	{
		public int Id { get; set; }
		[Display(Name = "Namn")]
		public string Name { get; set; }

		
	}
}