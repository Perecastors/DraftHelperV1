using FirstAPI.DbContext;
using FirstAPI.Models;
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
            ChampionSpellInfosViewModel csvm = CreateChampionSpellInfoViewModel(championInfo);
            return View(csvm);
        }

        private ChampionSpellInfosViewModel CreateChampionSpellInfoViewModel(ChampionJsonToObject obj)
        {
            ChampionSpellInfosViewModel csvm = new ChampionSpellInfosViewModel();
            csvm.id = obj.id;
            csvm.key = obj.key;
            csvm.name = obj.name;
            csvm.title = obj.title;
            csvm.spells = new List<Models.Spells>();
            foreach (var item in obj.spells)
            {
                var spell = new Models.Spells();
                spell.cooldown = new decimal[item.cooldown.Length];
                for(int i = 0; i < item.cooldown.Length; i++)
                {
                    string frenchNumber = item.cooldown[i].Replace('.', ',');
                    spell.cooldown[i] = decimal.Parse(frenchNumber);
                }
                spell.cooldownBurn = item.cooldownBurn;
                spell.name = item.name;
                spell.tooltip = item.tooltip;
                spell.image = new Models.Image();
                spell.image.full = item.image.full;
                spell.image.sprite = item.image.sprite;
                csvm.spells.Add(spell);
            };
            
            return csvm;
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
            public string name { get; set; }
            public string tooltip { get; set; }
            public String[] cooldown { get; set; }
            public Image image { get; set; }
        }

        public class Image
        {
            private string _full;
            public string full
            {
                get { return ConfigurationManager.AppSettings["UrlSpellChamp"] + _full; }
                set { _full = value; }
            }
            public string sprite { get; set; }
        }
    }
}