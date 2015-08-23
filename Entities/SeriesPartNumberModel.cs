using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class SeriesPartNumberModel
	{
		public int Id { get; set; }
		public int SeriesId { get; set; }
		public int PartNumber { get; set; }
	}
}
