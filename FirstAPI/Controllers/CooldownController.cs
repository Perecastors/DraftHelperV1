using FirstAPI.DbContext;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FirstAPI.Controllers
{
    public class CooldownController : Controller
    {
        // GET: Cooldown
        public ActionResult Index()
        {
            //ViewBag.ListChampions = SelectListHelper.getAllChampions();
            
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var champId = form["listChamp"];
            string champName = new DALChampionPool().GetChampionByChampionId(Guid.Parse(champId)).ChampionName;
            string json="";
            using (WebClient wc = new WebClient())
            {
                var url = ConfigurationManager.AppSettings["UrlChamp"] + champName + ".json";
                var json2 = wc.DownloadString(url);
                var obj = JObject.Parse(json2);
                json = obj.SelectToken("data").Last.Last.ToString();
            }
            var championInfo = JsonConvert.DeserializeObject<ChampionJsonToObject>(json);
            return View();
        }

        [Serializable]
        public class ChampionJsonToObject
        {
            public string id { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public string title { get; set; }
            public List<Spells> spells { get; set; }
        }

        public class Spells
        {
            public string cooldownBurn { get; set; }
            public String[] cooldown { get; set; }
        }
    }
}