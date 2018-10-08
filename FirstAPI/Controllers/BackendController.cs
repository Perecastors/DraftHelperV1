using FirstAPI.DbContext;
using FirstAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstAPI.Controllers
{
    public class BackendController : MyBaseController
    {
        // GET: Backend
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ImportChampionRiotId(List<ChampionRiot> championsList)
        {
                if (championsList?.Count > 0)
                {
                    var dal = new DAL();
                    foreach (var item in championsList)
                    {
                        dal.UpdateChampion(item.id, item.key);
                    }
                    return Json("Ok", JsonRequestBehavior.AllowGet);
                }
            return Json("Ko", JsonRequestBehavior.AllowGet);
        }
    }
}