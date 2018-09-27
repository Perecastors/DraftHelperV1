using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstAPI.WebHelperTools
{
    public static class SelectListHelper
    {
        public static SelectList getAllChampions()
        {
            var dal = new DAL();
            var list = dal.getAllChampions();
            var listSelectItem = new List<SelectListItem>();
            listSelectItem.Add(new SelectListItem() { Text = "", Value = "" });
            foreach (var item in list)
            {
                listSelectItem.Add(new SelectListItem() { Text = item.ChampionName, Value = item.ChampionId.ToString() });
            }
            var selectlist = new SelectList(listSelectItem,"Value","Text");
            return selectlist;
        }

        public static SelectList getAllChampions2()
        {
            var dal = new DAL();
            var list = dal.getAllChampions();
            var listSelectItem = new List<SelectListItem>();
            int count = 0;
            string groupName = list.ElementAtOrDefault(count).ChampionName[0] + " - " + list.ElementAtOrDefault(count+19)?.ChampionName[0];
            var group = new SelectListGroup() { Name = groupName, Disabled = false };
            char? lastItemName;
            foreach (var item in list)
            {
                if(count % 18 == 0)
                {
                    if (String.IsNullOrEmpty(list.ElementAtOrDefault(count + 19)?.ChampionName[0].ToString())){
                        lastItemName = 'Z';
                    }
                    else
                    {
                        lastItemName = list.ElementAtOrDefault(count + 19)?.ChampionName[0];
                    }
                    groupName = list.ElementAtOrDefault(count).ChampionName[0] + " - " + lastItemName;
                    group = new SelectListGroup() { Name = groupName, Disabled = false };
                }
                listSelectItem.Add(new SelectListItem() { Text = item.ChampionName, Value = item.ChampionId.ToString(),Group=group });
                count++;
            }
            var selectlist = new SelectList(listSelectItem, "Value", "Text", "Group.Name",null,null);
            return selectlist;
        }
    }
}