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
		private GetApiResponse<SeriesModel> apiModel;
		public SidebarController()
		{
			apiModel = new GetApiResponse<SeriesModel>();
		}
		public ActionResult Index()
		{
			var model = new List<SeriesModel>();
			model = apiModel.GetAllSeriesFromDb("api/Series?");
			return PartialView(model);
		}
	}
}