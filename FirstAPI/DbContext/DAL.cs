using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FirstAPI.DbContext
{
    public class DAL
    {
        Database1Entities db;
        public DAL()
        {
            db = new Database1Entities();
            if (ConfigurationManager.AppSettings["Environment"].ToLower() == "prod")
                db = new Database1Entities("name=Database2Entities");
        }

        //GET 
        public List<Player> getAllPlayers()
        {
            List<Player> listPlayers = new List<Player>();
            listPlayers.AddRange(db.Players.ToList().OrderBy(x => x.Nickname));
            return listPlayers;
        }

        public Player getPlayerByName(string name)
        {
            Player player = new Player();
            player = db.Players.Where(x => x.Nickname.ToLower().Equals(name.ToLower())).FirstOrDefault();
            return player;
        }

        public Player getPlayerById(Guid playerId)
        {
            Player player = new Player();
            player = db.Players.Where(x => x.PlayerId == playerId).FirstOrDefault();
            return player;
        }

        public List<Champion> getAllChampions()
        {
            List<Champion> listChampions = new List<Champion>();
            listChampions.AddRange(db.Champions.ToList().OrderBy(x => x.ChampionName));
            return listChampions;
        }

        public string getChampionNameById(Guid id)
        {
            string championName;
            championName= db.Champions.Where(x => x.ChampionId == id).FirstOrDefault()?.ChampionName;
            return championName;
        }


        //ADD
        public int AddChampion(Champion champion)
        {
            champion.ChampionId = Guid.NewGuid();
            db.Champions.Add(champion);
            int result = db.SaveChanges();
            return result;
        }

        public int AddPlayer(Player player)
        {
            if (player.Nickname == null)
                return 0;
            var existingPlayer = db.Players.Where(x => x.Nickname == player.Nickname).FirstOrDefault();
            if (existingPlayer != null)
                return 0;
            player.PlayerId = Guid.NewGuid();
            db.Players.Add(player);
            int result = db.SaveChanges();
            return result;
        }

        //DELETE
        public int DeletePlayer(Guid playerId)
        {
            var player = db.Players.Where(x => x.PlayerId == playerId).FirstOrDefault();
            if (player != null)
            {
                db.Players.Remove(player);
                int result = db.SaveChanges();
                return result;
            }
            return 0;
        }

        public int DeleteChampion(Guid championId)
        {
            var player = db.Champions.Where(x => x.ChampionId == championId).FirstOrDefault();
            if (player != null)
            {
                db.Champions.Remove(player);
                int result = db.SaveChanges();
                return result;
            }
            return 0;
        }
    }
}