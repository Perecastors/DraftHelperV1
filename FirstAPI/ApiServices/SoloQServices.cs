using FirstAPI.DbContext;
using FirstAPI.ApiServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace FirstAPI.ApiServices
{
    public class SoloQServices
    {
        public Player getPlayerAccount(string name)
        {
            //https://euw1.api.riotgames.com/lol/summoner/v4/summoners/by-name/perecastors?api_key=RGAPI-3198e533-71d3-4f2f-9dac-74bbd6459e73
            string json="";
            Account account = null;
            Player player = null;
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var url = ConfigurationManager.AppSettings["UrlAccount"] + name + "?&api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"]; ;
                    var stringJson = wc.DownloadString(url);
                    var obj = JObject.Parse(stringJson);
                    json = obj.ToString();
                    account = JsonConvert.DeserializeObject<Account>(json);
                }
            }
            catch (Exception)
            {
            }
            
            if(account != null)
            {
                player = ConvertAccountToPlayer(account);
            }
            return player;
        }

        private Player ConvertAccountToPlayer(Account account)
        {
            Player player = new Player();
            player.PlayerId = Guid.NewGuid();
            player.Nickname = account.name;
            player.AccountId = account.accountId;
            player.Role = 0;

            return player;
        }

        public List<Match> GetSoloQHistories(string accountId, int nbGamesToGet)
        {
            // omW_BRFWmEMp-FxJWq6Cd9wim2Ldlaqx8AFdeL5B0OFRhg => accountId de perecastors
            //"https://euw1.api.riotgames.com/lol/match/v4/matchlists/by-account/@(Model.AccountId)?endIndex=" + nbGames + "&queue=420&api_key=@(ConfigurationManager.AppSettings["ApiRiotKey"])"

            string json = "";
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    var url = ConfigurationManager.AppSettings["UrlMatchlist"] + accountId + "?queue=420&endIndex=" + nbGamesToGet + "&api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"];
                    var stringJson = wc.DownloadString(url);
                    var obj = JObject.Parse(stringJson);
                    json = obj.SelectToken("matches").ToString();
                }
            }
            catch (Exception e)
            {
            }
            
            var matches = JsonConvert.DeserializeObject<List<Match>>(json);
            return matches;
        }

        public AllRunes GetRunes()
        {
            string json = "";
            DALSoloQ dsq = new DALSoloQ();
            json = dsq.GetRunesJson(ConfigurationManager.AppSettings["PatchVersion"]);
            if (string.IsNullOrEmpty(json))
            {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        var url = ConfigurationManager.AppSettings["UrlRunes"].Replace("[patchversion]", ConfigurationManager.AppSettings["PatchVersion"]);
                        json = wc.DownloadString(url);
                        Thread.Sleep(100);
                        //dsq.SaveRunesJson(json, patchVersion);
                    }
            }
            var obj = JArray.Parse(json);
            json = obj.ToString();
            var runes = JsonConvert.DeserializeObject<SlotsJson[]>(json);
            var arunes = BuildAllRunes(runes);
            return arunes;
        }

        public MatchInfos GetMatchInfo(string gameId)
        {
            //4521496705 => gameId exemple
            string json = "";
            DALSoloQ dsq = new DALSoloQ();
            json = dsq.GetMatchInfos(gameId);
            if (string.IsNullOrEmpty(json))
            {
                int retry = 0;
                while (retry < 4)
                {
                    try
                    {
                        using (WebClient wc = new WebClient())
                        {
                            wc.Encoding = Encoding.UTF8;
                            var url = ConfigurationManager.AppSettings["UrlMatch"] + gameId + "?&api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"];
                            json = wc.DownloadString(url);
                            Thread.Sleep(100);
                            dsq.SaveMatchInfos(json, gameId);
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        string e = ex.Message;
                        if (e.ToLower().Contains("too many"))
                        {
                            Thread.Sleep(2000);
                            retry++;
                        }
                        if (e.ToLower().Contains("504"))
                        {
                            Thread.Sleep(2000);
                            retry++;
                        }
                        if (retry == 3)
                        {
                            throw;
                        }
                    }
                }
            }
            var obj = JObject.Parse(json);
            json = obj.ToString();
            var match = JsonConvert.DeserializeObject<MatchInfos>(json);
            return match;
        }

        public List<MatchInfos> GetListMatchInfos(List<Match> listMatch)
        {
            List<MatchInfos> listMatchInfos = new List<MatchInfos>();
            int nbGames = 0;
            foreach (var game in listMatch)
            {
                MatchInfos matchInfos = GetMatchInfo(game.gameId.ToString());
                if (nbGames % 20 == 0)
                {
                    Thread.Sleep(1000);
                }
                nbGames++;
                listMatchInfos.Add(matchInfos);
            }
            return listMatchInfos;
        }

        //timelanes = liste de frames ?? => apparament oui
        public List<Frame> GetTimeLinesMatchInfos(string gameId = "4521496705")
        {
            //4521496705 => gameId exemple
            string json = "";
            DALSoloQ dsq = new DALSoloQ();
            json = dsq.GetTimelinesMatchInfos(gameId);
            if (string.IsNullOrEmpty(json))
            {
                int retry = 0;
                while (retry < 4)
                {
                    try
                    {
                        using (WebClient wc = new WebClient())
                        {
                            wc.Encoding = Encoding.UTF8;
                            var url = ConfigurationManager.AppSettings["UrlTimelineMatch"] + gameId + "?&api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"];
                            json = wc.DownloadString(url);
                            Thread.Sleep(100);
                            dsq.SaveTimelinesMatchInfos(json, gameId);
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        string e = ex.Message;
                        if (e.ToLower().Contains("too many"))
                        {
                            Thread.Sleep(2000);
                            retry++;
                        }
                        if (e.ToLower().Contains("504"))
                        {
                            Thread.Sleep(2000);
                            retry++;
                        }
                        if (retry == 3)
                        {
                            throw;
                        }
                    }
                }
            }
            var obj = JObject.Parse(json);
            json = obj.SelectToken("frames").ToString();
            var timelinesJson = JsonConvert.DeserializeObject<List<FrameJson>>(json);
            var timeline = BuildListFrame(timelinesJson);//=> dans le json : c'est pas une liste de participantframe: cette méthode me permet de reconstruire l'objet
            return timeline;
        }

        public SummonerObject GetSummonersByIdKey(int key)
        {
            string json;
            using (WebClient wc = new WebClient())
            {
                var url = ConfigurationManager.AppSettings["UrlSummoner"];
                var stringJson = wc.DownloadString(url);
                var obj = JObject.Parse(stringJson);
                json = obj.SelectToken("data").ToString();
            }
            var summonerInfos = JsonConvert.DeserializeObject<SummonerInfoToObject>(json);
            var summoner = ConvertSummonerObjectsToList(summonerInfos);
            return summoner.Where(x => x.key == key.ToString()).FirstOrDefault();
        }

        private List<SummonerObject> ConvertSummonerObjectsToList(SummonerInfoToObject obj)
        {
            List<SummonerObject> list = new List<SummonerObject>();
            list.Add(obj.SummonerBarrier);
            list.Add(obj.SummonerBoost);
            list.Add(obj.SummonerDot);
            list.Add(obj.SummonerExhaust);
            list.Add(obj.SummonerFlash);
            list.Add(obj.SummonerHaste);
            list.Add(obj.SummonerHeal);
            list.Add(obj.SummonerSmite);
            list.Add(obj.SummonerTeleport);

            return list;
        }

        public string GetNicknameByAccountId(string accountId)
        {
            string json = "";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                var url = ConfigurationManager.AppSettings["UrlSummonerAccount"] + accountId + "?api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"];
                var stringJson = wc.DownloadString(url);
                var obj = JObject.Parse(stringJson);
                json = obj.ToString();
            }
            var summoner = JsonConvert.DeserializeObject<Summoner>(json);
            return summoner.summonerName;
        }

        public List<Frame> BuildListFrame(List<FrameJson> frameJsons)
        {
            List<Frame> frames = new List<Frame>();
            foreach (var frame in frameJsons)
            {
                Frame newFrame = new Frame();
                newFrame.timestamp = frame.timestamp;

                newFrame.events = new List<Event>();
                newFrame.events.AddRange(frame.events);

                newFrame.participantFrames = new List<ParticipantFrame>();
                newFrame.participantFrames.Add(frame.participantFrames.participantFrame1);
                newFrame.participantFrames.Add(frame.participantFrames.participantFrame2);
                newFrame.participantFrames.Add(frame.participantFrames.participantFrame3);
                newFrame.participantFrames.Add(frame.participantFrames.participantFrame4);
                newFrame.participantFrames.Add(frame.participantFrames.participantFrame5);
                newFrame.participantFrames.Add(frame.participantFrames.participantFrame6);
                newFrame.participantFrames.Add(frame.participantFrames.participantFrame7);
                newFrame.participantFrames.Add(frame.participantFrames.participantFrame8);
                newFrame.participantFrames.Add(frame.participantFrames.participantFrame9);
                newFrame.participantFrames.Add(frame.participantFrames.participantFrame10);

                frames.Add(newFrame);
            }

            return frames;
        }

        public AllRunes BuildAllRunes(SlotsJson[] slots)
        {
            AllRunes runes = new AllRunes();
            runes.slots = new List<Slots>();
            for (int i = 0; i < slots.Length; i++)
            {
                Slots slot = new Slots();
                slot.id = slots[i].id;
                slot.key = slots[i].key;
                slot.runes = new List<Rune>();
                for (int j = 0; j < slots[i].slots.Length; j++)
                {
                    
                    for (int k = 0; k < slots[i].slots[j].runes.Length; k++)
                    {
                        Rune rune = new Rune();
                        rune.icon = slots[i].slots[j].runes[k].icon;
                        rune.id = slots[i].slots[j].runes[k].id;
                        rune.key = slots[i].slots[j].runes[k].key;
                        slot.runes.Add(rune);
                    }
                    
                }
                runes.slots.Add(slot);
            }
            return runes;
        }
    }


}