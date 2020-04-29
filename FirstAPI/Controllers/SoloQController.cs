using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using FirstAPI.ApiServices;

namespace FirstAPI.Controllers
{
    [Authorize]
    public class SoloQController : MyBaseController
    {
        // GET: SoloQ
        public ActionResult SoloQHistory(Guid playerId)
        {
            var s = new SoloQServices();
            
            var dal = new DAL();
            var serializer = new JavaScriptSerializer();
            var player = dal.getPlayerById(playerId);
            //ViewBag.ChampionList = serializer.Serialize(((List<Champion>)(Session["GlobalChampions"])));
            //s.GetSoloQHistories(player.AccountId);
            //s.GetMatchInfo();
            //s.GetTimeLineMatchInfo();
            return View(player);

        }

        public ActionResult SoloQ2(Guid playerId)
        {
            var dal = new DAL();
            var player = dal.getPlayerById(playerId);
            return View(player);
        }



    }
}