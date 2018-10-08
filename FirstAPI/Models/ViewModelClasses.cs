using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.Models
{
    public class MatchupInfos
    {
        public Guid playerId { get; set; }
        public Guid matchupId { get; set; }
        public Guid matchupResponseId { get; set; }
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

        public Guid EnnemyTop { get; set; }
        public string EnnemyTopName { get { return convertGUIDtoString(EnnemyTop); } set { EnnemyTopName = value; } }
        public Guid EnnemyJungle { get; set; }
        public string EnnemyJungleName { get { return convertGUIDtoString(EnnemyJungle); } set { EnnemyJungleName = value; } }
        public Guid EnnemyMid { get; set; }
        public string EnnemyMidName { get { return convertGUIDtoString(EnnemyMid); } set { EnnemyMidName = value; } }

        public Guid EnnemyAdc { get; set; }
        public string EnnemyAdcName { get { return convertGUIDtoString(EnnemyAdc); } set { EnnemyAdcName = value; } }

        public Guid EnnemySupport { get; set; }
        public string EnnemySupportName { get { return convertGUIDtoString(EnnemySupport); } set { EnnemySupportName = value; } }

        public string PatchVersion { get; set; }

        public List<MatchupAnswer> answers { get; set; }

        private string convertGUIDtoString(Guid id)
        {
            return ((List<Champion>)System.Web.HttpContext.Current.Session["GlobalChampions"]).Where(x => x.ChampionId == id).FirstOrDefault()?.ChampionName;
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
        public bool FlexEnnemyTop { get; set; }
        public bool FlexEnnemyJungle { get; set; }
        public bool FlexEnnemyMid { get; set; }
        public bool FlexEnnemyAdc { get; set; }
        public bool FlexEnnemySupport { get; set; }

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

        public Guid EnnemyBan1 { get; set; }
        public Guid EnnemyBan2 { get; set; }
        public Guid EnnemyBan3 { get; set; }
        public Guid EnnemyBan4 { get; set; }
        public Guid EnnemyBan5 { get; set; }


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