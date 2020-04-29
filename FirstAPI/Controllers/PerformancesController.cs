using FirstAPI.ApiServices;
using FirstAPI.DbContext;
using FirstAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstAPI.Controllers
{
    public class PerformancesController : Controller
    {
        // GET: Performances
        public ActionResult Performances(Guid playerId)
        {
            PerformancesViewModelBuilder builder = new PerformancesViewModelBuilder();
            SoloQServices sq = new SoloQServices();
            var dal = new DAL();
            var player = dal.getPlayerById(playerId);
            var matches = sq.GetSoloQHistories(player.AccountId,40);
            List<PerformancesViewModel> lpvm = new List<PerformancesViewModel>();
            if(player != null)
            {
                lpvm = builder.BuildPerformanceViewModel(matches, player);
            }
            return View(lpvm);
        }
    }
}