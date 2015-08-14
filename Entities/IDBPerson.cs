using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
	public interface IDBPerson
	{
		int Id { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
	}
}
