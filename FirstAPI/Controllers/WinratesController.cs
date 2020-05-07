using FirstAPI.ApiServices;
using FirstAPI.DbContext;
using FirstAPI.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstAPI.Controllers
{
    public class WinratesController : Controller
    {
        // GET: Winrates
        public ActionResult Winrates(Guid playerId)
        {
            WinratesViewModel wrm = new WinratesViewModel();
            wrm.blueSide = new WinratesViewModel.Side();
            wrm.redSide = new WinratesViewModel.Side();

            SoloQServices sq = new SoloQServices();
            var dal = new DAL();
            var player = dal.getPlayerById(playerId);
            var matches = sq.GetSoloQHistories(player.AccountId, 100);
            var ps = new PerformanceServices();

            foreach (var match in matches)
            {

                var matchInfos = sq.GetMatchInfo(match.gameId.ToString());
                //savoir dans quel side il était et si il a win
                var team = ps.GetPlayerTeam(matchInfos, player);
                var isWin = matchInfos.teams.Where(x => x.teamId == team).FirstOrDefault().win;
                if (team == 100)
                {
                    if (isWin.ToLower() == "win")
                    {
                        wrm.blueSide.totalGames++;
                        wrm.blueSide.nbWin++;
                        wrm.totalGames++;
                    }
                    else if (isWin.ToLower() == "fail")
                    {
                        wrm.blueSide.totalGames++;
                        wrm.blueSide.nbLoss++;
                        wrm.totalGames++;
                    }

                }
                else if (team == 200)
                {
                    if (isWin.ToLower() == "win")
                    {
                        wrm.redSide.totalGames++;
                        wrm.redSide.nbWin++;
                        wrm.totalGames++;
                    }
                    else if (isWin.ToLower() == "fail")
                    {
                        wrm.redSide.totalGames++;
                        wrm.redSide.nbLoss++;
                        wrm.totalGames++;
                    }
                }
            }

            return View(wrm);
        }
    }
}