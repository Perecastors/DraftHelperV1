using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace FirstAPI.Controllers
{
    [Authorize]
    public class SoloQController : MyBaseController
    {
        // GET: SoloQ
        public ActionResult SoloQ(Guid playerId)
        {
            var dal = new DAL();
            var serializer = new JavaScriptSerializer();
            var player = dal.getPlayerById(playerId);
            ViewBag.ChampionList = serializer.Serialize(((List<Champion>)(Session["GlobalChampions"])));
            return View(player);

        }

    }
}