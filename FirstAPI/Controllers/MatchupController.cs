using FirstAPI.DbContext;
using FirstAPI.Models;
using FirstAPI.WebHelperTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstAPI.Controllers
{
    public class MatchupController : Controller
    {
        // GET: Matchup
        public ActionResult Matchup(Guid playerId)
        {
            ViewBag.PlayerId = playerId;
            var dal = new DAL();
            var champPoolDal = new DALChampionPool();
            var playerDal = new DAL();
            ViewBag.MyChampList = champPoolDal.getChampionPool(playerId);
            ViewBag.ListChampions = SelectListHelper.getAllChampions();
            var player = playerDal.getPlayerById(playerId);
            ViewBag.Role = player?.Role;
            ViewBag.Nickname = player?.Nickname;
            return View();
        }

        public ActionResult CustomMatchup(Guid playerId,Guid matchupId)
        {
            ViewBag.PlayerId = playerId;
            var dal = new DAL();
            var champPoolDal = new DALChampionPool();
            var playerDal = new DAL();
            ViewBag.MyChampList = champPoolDal.getChampionPool(playerId);
            ViewBag.ListChampions = SelectListHelper.getAllChampions();
            var player = playerDal.getPlayerById(playerId);
            ViewBag.Role = player?.Role;
            ViewBag.Nickname = player?.Nickname;
            if (matchupId != Guid.Empty)
            {
                var matchupDal = new DALMatchup();
                var matchup = matchupDal.getMatchupByMatchupId(matchupId);
                ViewBag.MatchupId = matchupId;
                return View("Matchup", matchup);
            }
            return RedirectToAction("Matchup" , new { matchupId = matchupId });
        }

        public ActionResult SearchMatchup(Guid playerId)
        {
            Session["GlobalChampions"] = new DAL().getAllChampions();
            ViewBag.PlayerId = playerId;
            //var matchupDal = new DALMatchup();
            var champPoolDal = new DALChampionPool();
            var playerDal = new DAL();
            var player = playerDal.getPlayerById(playerId);
            ViewBag.Role = player?.Role;
            ViewBag.Nickname = player?.Nickname;
            ViewBag.MyChampList = champPoolDal.getChampionPool(playerId);
            ViewBag.ListChampions = SelectListHelper.getAllChampions();
            //var listMatchupInfos = matchupDal.getAllMatchupByPlayerId(playerId);
            return View();
        }

        [HttpPost]
        public JsonResult CreateNewMatchup(Guid playerId,MatchupInfos obj)
        {
            var dalMatchup = new DALMatchup();
            var matchupId = dalMatchup.createNewMatchup(playerId, obj);
            if(matchupId != Guid.Empty)
            {
                return Json(matchupId,JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddMatchup(MatchupInfos obj)
        {
            var dal = new DALMatchup();
            int result = dal.AddMatchup(obj);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteMatchup(Guid matchupId,Guid matchupResponseId)
        {
            var dal = new DALMatchup();
            int result = dal.deleteMatchup(matchupId, matchupResponseId);

            return Json("",JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSearchResults(Guid playerId,MatchupInfos matchupInfos)
        {
            var matchupDal = new DALMatchup();
            Stopwatch s = new Stopwatch();
            s.Start();
            var listMatchupInfos = matchupDal.getAllMatchupByParams(playerId, matchupInfos);
            s.Stop();
            ViewBag.CalculationTime = listMatchupInfos.Item2 / 1000d;
            ViewBag.AutomaticResults = listMatchupInfos.Item3;
            return PartialView("SearchResults", listMatchupInfos.Item1);
        }

        [HttpPost]
        public ActionResult AutomaticSearchResults(Guid playerId, MatchupInfos matchupInfos)
        {
            var matchupDal = new DALMatchup();
            Stopwatch s = new Stopwatch();
            s.Start();
            matchupInfos.playerId = playerId;
            var listMatchupInfos = matchupDal.GetEstimatedAnswersByMatchupParam2(matchupInfos);
            s.Stop();
            //ViewBag.CalculationTime = listMatchupInfos.Item2 / 1000d;
            return PartialView("AutomaticResults", listMatchupInfos);
        }

        [HttpPost]
        public JsonResult UpdateSearchResults2(Guid playerId, MatchupInfos matchupInfos)
        {
            var matchupDal = new DALMatchup();
            var listMatchupInfos = matchupDal.getAllMatchupByParams(playerId, matchupInfos);

            return Json(listMatchupInfos, JsonRequestBehavior.AllowGet);
        }

    }

    
}