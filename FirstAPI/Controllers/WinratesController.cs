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
            var builder = new WinratesViewModelBuilder();
            var wrm = builder.BuildWinratesViewModel(50, playerId);

            return View(wrm);
        }
    }
}