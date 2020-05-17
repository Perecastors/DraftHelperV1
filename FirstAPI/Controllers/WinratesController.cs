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
        public ActionResult Winrates()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult Winrates(FormWinrateViewModel form)
        {
            var builder = new WinratesViewModelBuilder();
            WinratesViewModel wrm = null ;

            var nbGames = form.nbGames;
            if (form.nbGames > 80)
            {
                nbGames = 80;
            }
            if (form.nbGames <= 0)
            {
                nbGames = 10;
            }
            var nickname = form.nickname;
            if (String.IsNullOrEmpty(form.nickname))
            {
                wrm = new WinratesViewModel();
                wrm.nickname = "n/a";
                return PartialView("WinratesNoResults", wrm); // envoyer une page d'erreur spécifique(nickname invalide)
            }
            

            var sq = new SoloQServices();
            var player = sq.getPlayerAccount(nickname);

            if (player != null)
            {
                wrm = builder.BuildWinratesViewModel(nbGames, player);
                if (wrm == null || wrm.totalGamesOnlyMainRole==0)
                {
                    wrm = new WinratesViewModel();
                    wrm.nickname = player.Nickname;
                    return PartialView("WinratesNoResults", wrm); // envoyer une page d'erreur spécifique(nickname invalide)
                }
            }else
            {
                wrm = new WinratesViewModel();
                wrm.nickname = nickname;
                return PartialView("WinratesNoResults", wrm); // envoyer une page d'erreur spécifique(nickname invalide)
            }

            return PartialView("WinratesResults",wrm);
        }
    }
}