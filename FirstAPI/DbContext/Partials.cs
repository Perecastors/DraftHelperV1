using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FirstAPI.DbContext
{
    public partial class ChampionTag
    {
        public string ChampionName { get { return convertGUIDtoString(this.ChampionId); } set { this.ChampionName = value; } }

        private string convertGUIDtoString(Guid id)
        {
            return ((List<Champion>)System.Web.HttpContext.Current.Session["GlobalChampions"]).Where(x => x.ChampionId == id).FirstOrDefault()?.ChampionName;
        }
    }

    public partial class Matchup
    {
        public Matchup DuplicateWithNewIds(Matchup matchup,Guid newMatchupId,Guid newMatchupResponsId)
        {
            var newMatchup = new Matchup();
            newMatchup.AllyTop = matchup.AllyTop;
            newMatchup.AllyJungle = matchup.AllyJungle;
            newMatchup.AllyMid = matchup.AllyMid;
            newMatchup.AllyAdc = matchup.AllyAdc;
            newMatchup.AllySupport = matchup.AllySupport;
            newMatchup.EnemyTop = matchup.EnemyTop;
            newMatchup.EnemyJungle = matchup.EnemyJungle;
            newMatchup.EnemyMid = matchup.EnemyMid;
            newMatchup.EnemyAdc = matchup.EnemyAdc;
            newMatchup.EnemySupport = matchup.EnemySupport;
            newMatchup.PlayerId = matchup.PlayerId;
            newMatchup.CreationDate = DateTime.Now;
            
            newMatchup.CreationDate = DateTime.Now;
            newMatchup.PatchVersion = matchup.PatchVersion;
            if (String.IsNullOrEmpty(matchup.PatchVersion) || newMatchup.PatchVersion != ConfigurationManager.AppSettings["PatchVersion"])
            {
                newMatchup.PatchVersion = ConfigurationManager.AppSettings["PatchVersion"];
            }

            newMatchup.MatchupResponseId = newMatchupResponsId;
            newMatchup.MatchupId = newMatchupId;
            return newMatchup;
        }
    }
}