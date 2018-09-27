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
        public Guid allyTop { get ; set; }
        public string allyTopName { get { return convertGUIDtoString(allyTop); } set { allyTopName = value; } }
        public Guid allyJungle { get; set; }
        public string allyJungleName { get { return convertGUIDtoString(allyJungle); } set { allyJungleName = value; } }
        public Guid allyMid { get; set; }
        public string allyMidName { get { return convertGUIDtoString(allyMid); } set { allyMidName = value; } }
        public Guid allyAdc { get; set; }
        public string allyAdcName { get { return convertGUIDtoString(allyAdc); } set { allyAdcName = value; } }
        public Guid allySupport { get; set; }
        public string allySupportName { get { return convertGUIDtoString(allySupport); } set { allySupportName = value; } }

        public Guid ennemyTop { get; set; }
        public string ennemyTopName { get { return convertGUIDtoString(ennemyTop); } set { ennemyTopName = value; } }
        public Guid ennemyJungle { get; set; }
        public string ennemyJungleName { get { return convertGUIDtoString(ennemyJungle); } set { ennemyJungleName = value; } }
        public Guid ennemyMid { get; set; }
        public string ennemyMidName { get { return convertGUIDtoString(ennemyMid); } set { ennemyMidName = value; } }

        public Guid ennemyAdc { get; set; }
        public string ennemyAdcName { get { return convertGUIDtoString(ennemyAdc); } set { ennemyAdcName = value; } }

        public Guid ennemySupport { get; set; }
        public string ennemySupportName { get { return convertGUIDtoString(ennemySupport); } set { ennemySupportName = value; } }

        public List<MatchupAnswer> answers { get; set; }

        private string convertGUIDtoString(Guid id)
        {
            return ((List<Champion>)System.Web.HttpContext.Current.Session["GlobalChampions"]).Where(x => x.ChampionId == id).FirstOrDefault()?.ChampionName;
        }
    }

    public class MatchupAnswer
    {
        public Guid ChampionId { get; set; }
        public string ChampionName { get { return convertGUIDtoString(ChampionId); } set { this.ChampionName = value; } }
        public string Comments { get; set; }
        private string convertGUIDtoString(Guid id)
        {
            return ((List<Champion>)System.Web.HttpContext.Current.Session["GlobalChampions"]).Where(x => x.ChampionId == id).FirstOrDefault()?.ChampionName;
        }

    }
}