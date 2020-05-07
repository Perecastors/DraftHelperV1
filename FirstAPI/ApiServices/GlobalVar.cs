using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FirstAPI.ApiServices
{
    public static class GlobalVar
    {
        public static string getRoleById(int role)
        {
            string srole="";
            switch (role)
            {
                case 1:
                    srole="TOP";
                    break;
                case 2:
                    srole = "JUNGLE";
                    break;
                case 3:
                    srole = "MID";
                    break;
                case 4:
                    srole = "BOTTOM";
                    break;
                case 5:
                    srole = "SUPPORT";
                    break;
                default:
                    break;
            }
            return srole;
        }

        public static string GetChampionNameById(int championId)
        {
            var dal = new DAL();
            return dal.getChampionNameById(championId);
        }

        public static string ConvertTimestampToDatetime(long timestamp)
        {
            string datetime = "";
            double timestamp2 = timestamp / 1000;
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dt = dt.AddSeconds(timestamp2).ToLocalTime();
            datetime = dt.ToString("g");

            return datetime;
        }

        public static string GetUrlSummonerImageByKey(int key)
        {
            var sq = new SoloQServices();
            var spell = sq.GetSummonersByIdKey(key).Image.full;
            return spell;
            
        }

        public static string GetUrlImgRune(int keyStyle,int keyRune)
        {
            var sq = new SoloQServices();
            var allRune = sq.GetRunes();
            var runeTree = allRune.slots.Where(x => x.id == keyStyle).FirstOrDefault();
            var icon = runeTree.runes.Where(x => x.id == keyRune).FirstOrDefault().icon;
            var url = ConfigurationManager.AppSettings["UrlImgRunes"] + icon;
            return url;
        }
    }

}