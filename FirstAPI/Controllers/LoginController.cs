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
                Player utilisateur = new DAL().Login(viewModel.Nickname, viewModel.Password);
                if (utilisateur != null)
                {
                    FormsAuthentication.SetAuthCookie(utilisateur.PlayerId.ToString(), false);
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return Redirect("/");
                }

                ModelState.AddModelError(utilisateur.Nickname, "Login et/ou mot de passe incorrect(s)");
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