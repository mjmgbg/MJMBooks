using Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
namespace Data
{
	public class DBContextUser: IdentityDbContext<ApplicationUser>
	{
		public DBContextUser()
			: base("name=MJMBooks")
		{
		}

		public static DBContextUser Create()
		{
			return new DBContextUser();
		}
	}
}
