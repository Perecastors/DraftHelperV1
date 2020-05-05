using FirstAPI.ApiServices;
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
        public ActionResult CooldownChampion()
        {
            //ViewBag.ListChampions = SelectListHelper.getAllChampions();
            
            return View();
        }

        [HttpPost]
        public ActionResult CooldownChampion(FormCollection form)
        {
            var champId = form["listChamp"];
            string champName = new DALChampionPool().GetChampionByChampionId(Guid.Parse(champId)).ChampionName;
            string json="";
            using (WebClient wc = new WebClient())
            {
                var url = ConfigurationManager.AppSettings["UrlChamp"] + champName + ".json";
                var stringJson = wc.DownloadString(url);
                var obj = JObject.Parse(stringJson);
                json = obj.SelectToken("data").Last.Last.ToString();
            }
            var championInfo = JsonConvert.DeserializeObject<ChampionJsonToObject>(json);
            ChampionSpellInfosViewModel csvm = CreateChampionSpellInfoViewModel(championInfo);
            return View(csvm);
        }

        public ActionResult CooldownSummoner()
        {
            string json = "";
            using (WebClient wc = new WebClient())
            {
                var url = ConfigurationManager.AppSettings["UrlSummoner"];
                var stringJson = wc.DownloadString(url);
                var obj = JObject.Parse(stringJson);
                json = obj.SelectToken("data").ToString();
            }
            var summonerInfos = JsonConvert.DeserializeObject<SummonerInfoToObject>(json);
            List<SummonerInfosViewModel> lsivm = CreateSummonerInfosViewModel(summonerInfos);
            return View(lsivm);
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

        //Tu peux m'insulter quand tu verras que je ne saurais pas faire de reflexion d'objet
        private List<SummonerInfosViewModel> CreateSummonerInfosViewModel(SummonerInfoToObject obj)
        {
            List<SummonerInfosViewModel> lsivm = new List<SummonerInfosViewModel>();
            SummonerInfosViewModel sivm = new SummonerInfosViewModel();

            sivm.cooldown = Double.Parse(obj.SummonerBarrier.cooldown[0]);
            sivm.name = obj.SummonerBarrier.name;
            sivm.image = obj.SummonerBarrier.Image.full;
            lsivm.Add(sivm);

            sivm = new SummonerInfosViewModel();
            sivm.cooldown = Double.Parse(obj.SummonerBoost.cooldown[0]);
            sivm.name = obj.SummonerBoost.name;
            sivm.image = obj.SummonerBoost.Image.full;
            lsivm.Add(sivm);

            sivm = new SummonerInfosViewModel();
            sivm.cooldown = Double.Parse(obj.SummonerDot.cooldown[0]);
            sivm.name = obj.SummonerDot.name;
            sivm.image = obj.SummonerDot.Image.full;
            lsivm.Add(sivm);

            sivm = new SummonerInfosViewModel();
            sivm.cooldown = Double.Parse(obj.SummonerExhaust.cooldown[0]);
            sivm.name = obj.SummonerExhaust.name;
            sivm.image = obj.SummonerExhaust.Image.full;
            lsivm.Add(sivm);

            sivm = new SummonerInfosViewModel();
            sivm.cooldown = Double.Parse(obj.SummonerFlash.cooldown[0]);
            sivm.name = obj.SummonerFlash.name;
            sivm.image = obj.SummonerFlash.Image.full;
            lsivm.Add(sivm);

            sivm = new SummonerInfosViewModel();
            sivm.cooldown = Double.Parse(obj.SummonerHaste.cooldown[0]);
            sivm.name = obj.SummonerHaste.name;
            sivm.image = obj.SummonerHaste.Image.full;
            lsivm.Add(sivm);

            sivm = new SummonerInfosViewModel();
            sivm.cooldown = Double.Parse(obj.SummonerHeal.cooldown[0]);
            sivm.name = obj.SummonerHeal.name;
            sivm.image = obj.SummonerHeal.Image.full;
            lsivm.Add(sivm);

            sivm = new SummonerInfosViewModel();
            sivm.cooldown = Double.Parse(obj.SummonerSmite.cooldown[0]);
            sivm.name = obj.SummonerSmite.name;
            sivm.image = obj.SummonerSmite.Image.full;
            lsivm.Add(sivm);

            // summoner TP : les données ne sont pas suffisantes dans le json pour calculer correctement les cooldown
            //sivm = new SummonerInfosViewModel();
            //sivm.cooldown = Decimal.Parse(obj.SummonerTeleport.cooldown[0]);
            //sivm.name = obj.SummonerTeleport.name;
            //sivm.image = obj.SummonerTeleport.Image.full;
            //lsivm.Add(sivm);

            return lsivm;
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
            public ChampionImage image { get; set; }
        }

        public class ChampionImage
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