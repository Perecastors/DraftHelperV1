using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace FirstAPI.ApiServices
{
    public class SoloQServices
    {
        public List<Match> GetSoloQHistories(string accountId= "Fx3TG_xMdAhlTajp8iVv4eUuRa4HHudgaPi60Bbj7EfMvQ", int nbGamesToGet=2)
        {
            // omW_BRFWmEMp-FxJWq6Cd9wim2Ldlaqx8AFdeL5B0OFRhg => accountId de perecastors
            //"https://euw1.api.riotgames.com/lol/match/v4/matchlists/by-account/@(Model.AccountId)?endIndex=" + nbGames + "&queue=420&api_key=@(ConfigurationManager.AppSettings["ApiRiotKey"])"

            string json = "";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                var url = ConfigurationManager.AppSettings["UrlMatchlist"] + accountId + "?queue=420&endIndex=" + nbGamesToGet + "&api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"];
                var stringJson = wc.DownloadString(url);
                var obj = JObject.Parse(stringJson);
                json = obj.SelectToken("matches").ToString();
            }
            var matches = JsonConvert.DeserializeObject<List<Match>>(json);
            return matches;
        }

        public MatchInfos GetMatchInfo(string gameId= "4521496705")
        {
            //4521496705 => gameId exemple
            string json = "";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                var url = ConfigurationManager.AppSettings["UrlMatch"] + gameId + "?&api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"];
                var stringJson = wc.DownloadString(url);
                var obj = JObject.Parse(stringJson);
                json = obj.ToString();
            }
            var match = JsonConvert.DeserializeObject<MatchInfos>(json);
            return match;
        }

        public void GetTimeLineMatchInfo(string gameId = "4521496705")
        {
            //4521496705 => gameId exemple
            string json = "";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                var url = ConfigurationManager.AppSettings["UrlTimelineMatch"] + gameId + "?&api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"];
                var stringJson = wc.DownloadString(url);
                var obj = JObject.Parse(stringJson);
                json = obj.SelectToken("frames").ToString();
            }
            var Timelinematch = JsonConvert.DeserializeObject<List<Frame>>(json);
        }
    }

   
}