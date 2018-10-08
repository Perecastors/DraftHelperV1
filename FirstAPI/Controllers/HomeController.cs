using FirstAPI.DbContext;
using FirstAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstAPI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult AddPlayer()
        {
            var dal = new DAL();
            ViewBag.ListPlayers = dal.getAllPlayers();
            ViewBag.ListRole = Enum.GetValues(typeof(roleList))
                .Cast<roleList>()
                .Select(t => new roleClass
                {
                    role = (int)t,
                    roleName = t.ToString()
                });
            ViewBag.Error = "";
            return View();
        }
        [HttpPost]
        public ActionResult AddPlayer(Player player)
        {
            var dal = new DAL();
            if (dal.AddPlayer(player) == 1)
                return RedirectToAction("AddPlayer");
            ViewBag.Error = " Error When adding player";
            return RedirectToAction("AddPlayer");
        }

        public ActionResult DeletePlayer(Guid id)
        {
            var dal = new DAL();
            if (dal.DeletePlayer(id) == 1)
                return RedirectToAction("AddPlayer");
            ViewBag.Error = " Error When deleting player";
            return RedirectToAction("AddPlayer");
        }

        public ActionResult AddChampion()
        {
            var dal = new DAL();
            ViewBag.ListChampions = dal.getAllChampions();
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public ActionResult AddChampion(Champion champion)
        {
            var dal = new DAL();
            if (dal.AddChampion(champion) == 1)
                return RedirectToAction("AddChampion");
            ViewBag.Error = " Error When adding champion";
            return RedirectToAction("AddChampion");
        }

       

        public ActionResult DeleteChampion(Guid id)
        {
            var dal = new DAL();
            if (dal.DeleteChampion(id) == 1)
                return RedirectToAction("AddChampion");
            ViewBag.Error = " Error When deleting player";
            return RedirectToAction("AddChampion");
        }

    }
}
