using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstAPI.Controllers
{
    public class MyBaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if(Session["Nickname"] == null)
            {
                var playerId = System.Web.HttpContext.Current.User.Identity.Name;
                if (!String.IsNullOrEmpty(playerId))
                {
                    var dal = new DAL();
                    var player = dal.getPlayerById(Guid.Parse(playerId));
                    if(player != null)
                    {
                        Session["Nickname"] = new DAL().getPlayerById(player.PlayerId).Nickname;
                    }
                }
            }
            if (Session["GlobalChampions"] == null)
            {
                Session["GlobalChampions"] = new DAL().getAllChampions();
            }
            //base.OnActionExecuted(filterContext);
        }
    }
}