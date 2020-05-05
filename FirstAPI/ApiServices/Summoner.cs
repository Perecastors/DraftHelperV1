using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FirstAPI.ApiServices
{
    public class SummonerInfoToObject
    {
        public SummonerObject SummonerBarrier { get; set; }
        public SummonerObject SummonerBoost { get; set; }//cleanse
        public SummonerObject SummonerDot { get; set; }//ignite
        public SummonerObject SummonerExhaust { get; set; }
        public SummonerObject SummonerFlash { get; set; }
        public SummonerObject SummonerTeleport { get; set; }
        public SummonerObject SummonerSmite { get; set; }
        public SummonerObject SummonerHeal { get; set; }
        public SummonerObject SummonerHaste { get; set; }

    }


    public class SummonerObject
    {
        public string name { get; set; }
        public string[] cooldown { get; set; }
        public string key { get; set; }
        public SummonerImage Image { get; set; }
    }

    public class SummonerImage
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