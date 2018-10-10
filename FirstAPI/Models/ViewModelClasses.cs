using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FirstAPI.Models
{
    public class MatchupInfos
    {
        public Guid PlayerId { get; set; }
        public Guid MatchupId { get; set; }
        public Guid MatchupResponseId { get; set; }
        public Guid AllyTop { get; set; }
        public string AllyTopName { get { return convertGUIDtoString(AllyTop); } set { AllyTopName = value; } }
        public Guid AllyJungle { get; set; }
        public string AllyJungleName { get { return convertGUIDtoString(AllyJungle); } set { AllyJungleName = value; } }
        public Guid AllyMid { get; set; }
        public string AllyMidName { get { return convertGUIDtoString(AllyMid); } set { AllyMidName = value; } }
        public Guid AllyAdc { get; set; }
        public string AllyAdcName { get { return convertGUIDtoString(AllyAdc); } set { AllyAdcName = value; } }
        public Guid AllySupport { get; set; }
        public string AllySupportName { get { return convertGUIDtoString(AllySupport); } set { AllySupportName = value; } }

        public Guid EnemyTop { get; set; }
        public string EnemyTopName { get { return convertGUIDtoString(EnemyTop); } set { EnemyTopName = value; } }
        public Guid EnemyJungle { get; set; }
        public string EnemyJungleName { get { return convertGUIDtoString(EnemyJungle); } set { EnemyJungleName = value; } }
        public Guid EnemyMid { get; set; }
        public string EnemyMidName { get { return convertGUIDtoString(EnemyMid); } set { EnemyMidName = value; } }

        public Guid EnemyAdc { get; set; }
        public string EnemyAdcName { get { return convertGUIDtoString(EnemyAdc); } set { EnemyAdcName = value; } }

        public Guid EnemySupport { get; set; }
        public string EnemySupportName { get { return convertGUIDtoString(EnemySupport); } set { EnemySupportName = value; } }

        public string PatchVersion { get; set; }

        public List<MatchupAnswer> Answers { get; set; }

        private string convertGUIDtoString(Guid id)
        {
            return ((List<Champion>)System.Web.HttpContext.Current.Session["GlobalChampions"]).Where(x => x.ChampionId == id).FirstOrDefault()?.ChampionName;
        }

        public MatchupInfos DuplicateMe(MatchupInfos matchupInfos)
        {
            MatchupInfos newMatchupInfo = new Models.MatchupInfos();
            newMatchupInfo.AllyTop = matchupInfos.AllyTop;
            newMatchupInfo.AllyJungle = matchupInfos.AllyJungle;
            newMatchupInfo.AllyMid = matchupInfos.AllyMid;
            newMatchupInfo.AllyAdc = matchupInfos.AllyAdc;
            newMatchupInfo.AllySupport = matchupInfos.AllySupport;
            newMatchupInfo.EnemyTop = matchupInfos.EnemyTop;
            newMatchupInfo.EnemyJungle = matchupInfos.EnemyJungle;
            newMatchupInfo.EnemyMid = matchupInfos.EnemyMid;
            newMatchupInfo.EnemyAdc = matchupInfos.EnemyAdc;
            newMatchupInfo.EnemySupport = matchupInfos.EnemySupport;
            newMatchupInfo.PlayerId = matchupInfos.PlayerId;
            newMatchupInfo.PatchVersion = matchupInfos.PatchVersion;
            if (String.IsNullOrEmpty(matchupInfos.PatchVersion) || matchupInfos.PatchVersion != ConfigurationManager.AppSettings["PatchVersion"])
            {
                newMatchupInfo.PatchVersion = ConfigurationManager.AppSettings["PatchVersion"];
            }
            newMatchupInfo.Answers = new List<MatchupAnswer>();
            if(matchupInfos.Answers?.Count() > 0)
            {
                newMatchupInfo.Answers.AddRange(matchupInfos.Answers);
            }
            return newMatchupInfo;
        }
    }

    public class MatchupAnswer
    {
        public Guid MatchupCommentId { get; set; }
        public Guid ChampionId { get; set; }
        public string ChampionName { get { return convertGUIDtoString(ChampionId); } set { this.ChampionName = value; } }
        public string Comments { get; set; }
        private string convertGUIDtoString(Guid id)
        {
            return ((List<Champion>)System.Web.HttpContext.Current.Session["GlobalChampions"]).Where(x => x.ChampionId == id).FirstOrDefault()?.ChampionName;
        }

    }

    public class DraftInfos : MatchupInfos
    {
        public bool FlexEnemyTop { get; set; }
        public bool FlexEnemyJungle { get; set; }
        public bool FlexEnemyMid { get; set; }
        public bool FlexEnemyAdc { get; set; }
        public bool FlexEnemySupport { get; set; }

        public bool FlexAllyTop { get; set; }
        public bool FlexAllyJungle { get; set; }
        public bool FlexAllyMid { get; set; }
        public bool FlexAllyAdc { get; set; }
        public bool FlexAllySupport { get; set; }

        public Guid AllyBan1 { get; set; }
        public Guid AllyBan2 { get; set; }
        public Guid AllyBan3 { get; set; }
        public Guid AllyBan4 { get; set; }
        public Guid AllyBan5 { get; set; }

        public Guid EnemyBan1 { get; set; }
        public Guid EnemyBan2 { get; set; }
        public Guid EnemyBan3 { get; set; }
        public Guid EnemyBan4 { get; set; }
        public Guid EnemyBan5 { get; set; }


        private string convertGUIDtoString(Guid id)
        {
            return ((List<Champion>)System.Web.HttpContext.Current.Session["GlobalChampions"]).Where(x => x.ChampionId == id).FirstOrDefault()?.ChampionName;
        }
    }

    public class ChampionComparer : IEqualityComparer<Champion>
    {
        public bool Equals(Champion x, Champion y)
        {
            return x.ChampionId == y.ChampionId;
        }

        public int GetHashCode(Champion obj)
        {
            return obj.ToString().GetHashCode();
        }
    }

    public class ChampionRiot
    {
        public string key { get; set; }
        public string id { get; set; }
    }

   
}