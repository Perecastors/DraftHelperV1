using System;
using System.Collections.Generic;
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
}