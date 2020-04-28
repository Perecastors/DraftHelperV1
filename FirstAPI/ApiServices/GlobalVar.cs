using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
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

        public static string getChampionNameById(int championId)
        {
            var dal = new DAL();
            return dal.getChampionNameById(championId);
        }
    }

}