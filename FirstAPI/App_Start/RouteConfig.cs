using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FirstAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Winrates",
                url: "Winrates/{playerId}",
                defaults: new { controller = "Winrates", action = "Winrates", playerId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CooldownSummoner",
                url: "CooldownSummoner/{playerId}",
                defaults: new { controller = "Cooldown", action = "CooldownSummoner", playerId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CooldownChampion",
                url: "CooldownChampion/{playerId}",
                defaults: new { controller = "Cooldown", action = "CooldownChampion", playerId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Performances",
                url: "Performances/{action}/{playerId}",
                defaults: new { controller = "Performances", action = "Performances", playerId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Tag",
                url: "Tag/{action}/{playerId}",
                defaults: new { controller = "Tag", action = "Tag", playerId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Backend",
                url: "Backend/{action}/{id}",
                defaults: new { controller = "Backend", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SoloQ",
                url: "SoloQ/{action}/{playerId}",
                defaults: new { controller = "SoloQ", action = "SoloQ", playerId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Login",
                url: "Login/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Draft",
                url: "Draft/{action}/{id}",
                defaults: new { controller = "Draft", action = "Draft", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Matchup",
                url: "Matchup/{action}/{playerId}",
                defaults: new { controller = "Matchup", action = "Matchup", playerId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ChampionPool",
                url: "ChampionPool/{action}/{playerId}",
                defaults: new { controller = "ChampionPool", action = "ChampionPool", playerId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}
