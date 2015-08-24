using Entities;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
	public class SidebarController : Controller
	{
		private GetApiResponse<SeriesModel> seriesApiModel;
		private GetApiResponse<AuthorModel> authorsApiModel;
		private GetApiResponse<ReaderModel> readersApiModel;
		public SidebarController()
		{
			seriesApiModel = new GetApiResponse<SeriesModel>();
			authorsApiModel = new GetApiResponse<AuthorModel>();
			readersApiModel = new GetApiResponse<ReaderModel>();
		}
		public ActionResult Series()
		{
			var model = new List<SeriesModel>();
			model = seriesApiModel.GetAllSeriesFromDb("api/Series?");
			return PartialView("_Series",model);
		}
		public ActionResult Authors()
		{
			var model = new List<AuthorModel>();
			model = authorsApiModel.GetAllAuthorsFromDb("api/Authors?");
			return PartialView("_Authors",model);
		}
		public ActionResult Readers()
		{
			var model = new List<ReaderModel>();
			model = readersApiModel.GetAllReadersFromDb("api/Readers?");
			return PartialView("_Readers",model);
		}
	}
}