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
            
            var nbGames = form.nbGames;
            if (form.nbGames > 50)
            {
                nbGames = 50;
            }
            if (form.nbGames <= 0)
            {
                nbGames = 1;
            }
            var nickname = form.nickname;
            if (String.IsNullOrEmpty(form.nickname))
            {
                return PartialView("WinratesResults", null); // envoyer une page d'erreur spécifique(nickname invalide)
            }
            

            var sq = new SoloQServices();
            var player = sq.getPlayerAccount(nickname);

            WinratesViewModel wrm = null ;
            if (player != null)
            {
                wrm = builder.BuildWinratesViewModel(nbGames, player);
            }else
            {
               return PartialView("WinratesResults", null); // envoyer une page d'erreur spécifique(nickname invalide)
            }

            return PartialView("WinratesResults",wrm);
        }
    }
}