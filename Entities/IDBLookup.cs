using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
	public interface IDBLookup
	{
		int Id { get; set; }
		string Name { get; set; }
	}
}