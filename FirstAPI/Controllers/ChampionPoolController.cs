using FirstAPI.DbContext;
using FirstAPI.WebHelperTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstAPI.Controllers
{
    public class ChampionPoolController : Controller
    {
        // GET: ChampionPool
        public ActionResult ChampionPool(Guid playerId)
        {
            var dal = new DALChampionPool();
            var MyChampionPool = dal.getChampionPool(playerId);
            ViewBag.ListChampions = SelectListHelper.getAllChampions();
            ViewBag.PlayerId = playerId;

            return View(MyChampionPool);
        }

        [HttpPost]
        public ActionResult AddChampion(Guid championId,Guid playerId)
        {
            var dal = new DALChampionPool();
            int result = dal.AddChampion(championId, playerId);
            //TODO : manage result value
            return RedirectToAction("ChampionPool", new { playerId = playerId });
        }

        public ActionResult DeleteChampion(Guid championId, Guid playerId)
        {
            var dal = new DALChampionPool();
            int result = dal.DeleteChampion(championId, playerId);
            //TODO : manage result value
            return RedirectToAction("ChampionPool", new { playerId = playerId });
        }
    }
}