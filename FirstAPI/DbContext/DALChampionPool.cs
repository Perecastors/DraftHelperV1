using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FirstAPI.DbContext
{
    public class DALChampionPool
    {
        static Database1Entities db;
        public DALChampionPool()
        {
            if (ConfigurationManager.AppSettings["Environment"].ToLower() == "prod")
                db = new Database1Entities("name=Database2Entities");
            else
                db = new Database1Entities();
        }

        public List<Champion> getChampionPool(Guid playerId)
        {
            var myChampionPool = new List<Champion>();
            var mychampList = db.ChampionPools.Where(x => x.PlayerId == playerId).ToList();
            foreach (var item in mychampList)
            {
                Champion champ = db.Champions.Where(x => x.ChampionId == item.ChampionId).FirstOrDefault();
                myChampionPool.Add(champ);
            }

            return myChampionPool?.OrderBy(x => x.ChampionName).ToList();
        }

        public int AddChampion(Guid championId, Guid playerId)
        {
            if (championId == null || championId == Guid.Empty)
                return 0;
            if (playerId == null || playerId == Guid.Empty)
                return 0;

            int result = 0;
            ChampionPool championPool = new ChampionPool();

            //Loof for existing championPoolId by playerId
            var existingchampionPool = db.ChampionPools.Where(x => x.PlayerId == playerId).FirstOrDefault();
            if (existingchampionPool != null)
            {
                //Look for existing champion in list
                bool alreadyAdded = db.ChampionPools.Where(x => x.ChampionPoolId == existingchampionPool.ChampionPoolId && x.PlayerId == playerId && x.ChampionId == championId).Any();
                if (alreadyAdded)
                    return 0;
            }

            //INIT
            championPool.ChampionId = championId;
            championPool.PlayerId = playerId;

            if (existingchampionPool?.ChampionPoolId == null || existingchampionPool?.ChampionPoolId == Guid.Empty)
            {
                championPool.ChampionPoolId = Guid.NewGuid();
            }
            else
            {
                championPool.ChampionPoolId = existingchampionPool.ChampionPoolId;
            }

            db.ChampionPools.Add(championPool);
            result = db.SaveChanges();

            return result;
        }

        public int DeleteChampion(Guid championId, Guid playerId)
        {
            if (championId == null || championId == Guid.Empty)
                return 0;
            if (playerId == null || playerId == Guid.Empty)
                return 0;

            int result = 0;

            var objtoDelete = db.ChampionPools.Where(x => x.ChampionId == championId && x.PlayerId == playerId).FirstOrDefault();
            if (objtoDelete != null)
            {
                db.ChampionPools.Remove(objtoDelete);
                result = db.SaveChanges();
            }

            return result;
        }
    }
}