using FirstAPI.DbContext;
using FirstAPI.WebHelperTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstAPI.Controllers
{
    public class TagController : Controller
    {
        // GET: Tag
        public ActionResult Tag(Guid playerId)
        {
            var dalChampionPool = new DALChampionPool();
            var dalTag = new DALTag();

            ViewBag.MyChampionPool = dalChampionPool.getChampionPool(playerId);
            ViewBag.PlayerId = playerId;
            ViewBag.Nickname = new DAL().getPlayerById(playerId).Nickname;
            ViewBag.ListTags = SelectListHelper.getAllTags(playerId);

            var tags = dalTag.GetAllTagsByPlayerId(playerId);
            return View(tags);
        }

        [HttpPost]
        public ActionResult AddTag(string tagName, Guid playerId)
        {
            var dal = new DALTag();
            int result = dal.AddTag(tagName, playerId);
            //TODO : manage result value
            return RedirectToAction("Tag", new { playerId = playerId });
        }

        public ActionResult DeleteTag(Guid tagId, Guid playerId)
        {
            var dal = new DALTag();
            int result = dal.DeleteTag(tagId, playerId);
            //TODO : manage result value
            return RedirectToAction("Tag", new { playerId = playerId });
        }

        [HttpPost]
        public ActionResult SaveTag(List<ChampionTag> championTagList,Guid playerId)
        {
            var dalTag = new DALTag();
            //int result = dalTag.SaveTag(championTagList);
            return RedirectToAction("Tag",new { playerId = playerId });
        }
    }
}