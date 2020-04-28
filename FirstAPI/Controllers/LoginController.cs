using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FirstAPI.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(Player viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var dal = new DAL();
                Player player = dal.Login(viewModel.Nickname, viewModel.Password);
                if (player != null)
                {
                    Session["GlobalChampions"] = dal.getAllChampions();
                    Session["Nickname"] = dal.getPlayerById(player.PlayerId).Nickname;
                    FormsAuthentication.SetAuthCookie(player.PlayerId.ToString(), false);
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return Redirect("/");
                }
            }
            return View(viewModel);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Login/Login");
        }
    }
}