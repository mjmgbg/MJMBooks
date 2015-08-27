using Entities;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
	public class AuthorController : Controller
	{
		private GetApiResponse<AuthorModel> apiModelBook;
		public AuthorController()
		{
			apiModelBook = new GetApiResponse<AuthorModel>();

		}
		// GET: Author
		public ActionResult Index()
		{
			var model = new List<AuthorModel>();
			model = apiModelBook.GetAllAuthorsFromDb("api/Author?");
			return PartialView(model);
		}
	}
}