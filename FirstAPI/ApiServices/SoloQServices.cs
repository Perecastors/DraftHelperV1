using FirstAPI.DbContext;
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
        public List<Match> GetSoloQHistories(string accountId, int nbGamesToGet = 2)
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

        public MatchInfos GetMatchInfo(string gameId)
        {
            //4521496705 => gameId exemple
            string json = "";
            DALSoloQ dsq = new DALSoloQ();
            json = dsq.GetMatchInfos(gameId);
            if (string.IsNullOrEmpty(json))
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    var url = ConfigurationManager.AppSettings["UrlMatch"] + gameId + "?&api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"];
                    json = wc.DownloadString(url);
                    dsq.SaveMatchInfos(json,gameId);
                }
            }
            var obj = JObject.Parse(json);
            json = obj.ToString();
            var match = JsonConvert.DeserializeObject<MatchInfos>(json);
            return match;
        }

        //timelanes = liste de frames ?? => apparament oui
        public List<Frame> GetTimeLinesMatchInfos(string gameId)
        {
            //4521496705 => gameId exemple
            string json = "";
            DALSoloQ dsq = new DALSoloQ();
            json = dsq.GetTimelinesMatchInfos(gameId);
            if (string.IsNullOrEmpty(json))
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    var url = ConfigurationManager.AppSettings["UrlTimelineMatch"] + gameId + "?&api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"];
                    json = wc.DownloadString(url);
                    dsq.SaveTimelinesMatchInfos(json,gameId);
                }
            }
            var obj = JObject.Parse(json);
            json = obj.SelectToken("frames").ToString();
            var timelines = JsonConvert.DeserializeObject<List<Frame>>(json);
            return timelines;
        }
    }


}