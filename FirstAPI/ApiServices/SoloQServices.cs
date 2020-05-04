using FirstAPI.DbContext;
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
        public List<Match> GetSoloQHistories(string accountId, int nbGamesToGet)
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

        public string GetNicknameByAccountId(string accountId)
        {
            string json = "";
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                var url = ConfigurationManager.AppSettings["UrlSummoner"] + accountId + "?api_key=" + ConfigurationManager.AppSettings["ApiRiotKey"];
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
    }


}