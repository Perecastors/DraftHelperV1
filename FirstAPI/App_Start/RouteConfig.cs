﻿using System;
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

            //routes.MapRoute(
            //    name: "Matchup2",
            //    url: "Matchup/{action}/{playerId}/{matchupId}",
            //    defaults: new { controller = "Matchup", action = "Matchup", playerId = UrlParameter.Optional, matchupId=UrlParameter.Optional }
            //);

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
