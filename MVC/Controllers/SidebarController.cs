using System.Configuration;
using System.Web.Mvc;
using Business;
using Business.DTO;

namespace MVC.Controllers
{
    public class SidebarController : Controller
    {
        private readonly GetApiResponse<NameViewModel> _nameApiModel;
        private readonly GetApiResponse<PersonViewModel> _personApiModel;

        public SidebarController()
        {
            _nameApiModel = new GetApiResponse<NameViewModel>(ConfigurationManager.AppSettings["apiBaseUri"]);
            _personApiModel = new GetApiResponse<PersonViewModel>(ConfigurationManager.AppSettings["apiBaseUri"]);
        }

        public ActionResult Series()
        {
            var model = _nameApiModel.GetAllNamesFromDbNotAsync("api/Serie?");
            return PartialView("_Series", model);
        }

        public ActionResult Authors()
        {
            var model = _personApiModel.GetAllPersonsFromDbNotAsync("api/Author?");
            return PartialView("_Authors", model);
        }

        public ActionResult Readers()
        {
            var model = _nameApiModel.GetAllPersonsFromDbNotAsync("api/Reader?");
            return PartialView("_Readers", model);
        }
    }
}