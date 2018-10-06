using FirstAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FirstAPI.DbContext
{
    public class DALCalculation
    {
        Database1Entities db;
        public DALCalculation()
        {
            db = new Database1Entities();
            if (ConfigurationManager.AppSettings["Environment"].ToLower() == "prod")
                db = new Database1Entities("name=Database2Entities");
        }



        public bool isPerfectMatchExist(MatchupInfos matchupInfos, Guid playerId)
        {
            var listMatchup = db.Matchups.Where(x => x.PlayerId == playerId);
            listMatchup = listMatchup.Where(x => x.AllyTop == matchupInfos.AllyTop);
            listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
            listMatchup = listMatchup.Where(x => x.AllyMid == matchupInfos.AllyMid);
            listMatchup = listMatchup.Where(x => x.AllyAdc == matchupInfos.AllyAdc);
            listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
            listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnnemyTop);
            listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnnemyJungle);
            listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnnemyMid);
            listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc);
            listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnnemySupport);

            if (listMatchup.Count() > 0)
                return true;

            return false;
        }

        public MatchupInfos GetEstimatedAnswersWithAllParam(MatchupInfos matchupInfos)
        {
            IQueryable<Matchup> listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
            List<Matchup> calcultedList = new List<Matchup>();

            bool hasFilter = false;
            //ALLY
            if (matchupInfos.AllyTop != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.AllyTop == matchupInfos.AllyTop)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.AllyJungle != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.AllyMid != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.AllyMid == matchupInfos.AllyMid)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.AllyAdc != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.AllyAdc == matchupInfos.AllyAdc)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.AllySupport != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport)).ToList();
                hasFilter = true;
            }
            //ENNEMI
            if (matchupInfos.EnnemyTop != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyTop == matchupInfos.EnnemyTop)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.EnnemyJungle != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnnemyJungle)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.EnnemyMid != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyMid == matchupInfos.EnnemyMid)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.EnnemyAdc != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.EnnemySupport != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemySupport == matchupInfos.EnnemySupport)).ToList();
                hasFilter = true;
            }

            //recuperer la liste das matchupResponsesId pour recuperer la liste des champions
            var listMatchupResponse = calcultedList.Select(x => x.MatchupResponseId).ToList();
            var automaticMatchupInfos = new MatchupInfos();
            automaticMatchupInfos = CreateAutomaticMatchupInfo(matchupInfos, listMatchupResponse);
            //listMatchupInfos.Add(listMatchup);
            //

            return automaticMatchupInfos;
        }

        public MatchupInfos GetEstimatedAnswersByMatchupParam(MatchupInfos matchupInfos)
        {
            var player = new DAL().getPlayerById(matchupInfos.playerId);
            IEnumerable<Matchup> listMatchup = GetListMatchupByRole(player, matchupInfos);
            bool hasFilter = false;
            if (listMatchup.Count() == 0)
            {
                listMatchup = Enumerable.Empty<Matchup>().AsQueryable();
                //ALLY
                if (matchupInfos.AllyTop != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyTop == matchupInfos.AllyTop && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList).ToList();
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyTop == matchupInfos.AllyTop && x.PlayerId == matchupInfos.playerId).ToList();
                    }
                    hasFilter = true;
                }
                if (matchupInfos.AllyJungle != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList).ToList();
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyJungle == matchupInfos.AllyJungle && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.AllyMid != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyMid == matchupInfos.AllyMid && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyMid == matchupInfos.AllyMid && x.PlayerId == matchupInfos.playerId);

                    }
                    hasFilter = true;
                }
                if (matchupInfos.AllyAdc != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyAdc == matchupInfos.AllyAdc && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyAdc == matchupInfos.AllyAdc && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.AllySupport != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllySupport == matchupInfos.AllySupport && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                //ENNEMI
                if (matchupInfos.EnnemyTop != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnnemyTop && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyTop == matchupInfos.EnnemyTop && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.EnnemyJungle != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnnemyJungle && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyJungle == matchupInfos.EnnemyJungle && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.EnnemyMid != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnnemyMid && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyMid == matchupInfos.EnnemyMid && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.EnnemyAdc != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.EnnemySupport != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnnemySupport && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemySupport == matchupInfos.EnnemySupport && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
            }else
            {
                hasFilter = true;
            }

            var automaticMatchupInfos = new MatchupInfos();
            if (hasFilter && listMatchup.Count() > 0)
            {
                //recuperer la liste das matchupResponsesId pour recuperer la liste des champions
                var listMatchupResponse = listMatchup.Select(x => x.MatchupResponseId).ToList();
                automaticMatchupInfos = CreateAutomaticMatchupInfo(matchupInfos, listMatchupResponse);
            }

            return automaticMatchupInfos;
        }

        public IEnumerable<Matchup> GetListMatchupByRole(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            switch (player.Role)
            {
                case (int)roleList.Top:
                    listMatchup = GetEstimatedAnswersAsToplaner(player, matchupInfos);
                    break;
                case (int)roleList.Mid:
                    listMatchup = GetEstimatedAnswersAsMidlaner(player, matchupInfos);
                    break;
                case (int)roleList.Adc:
                    listMatchup = GetEstimatedAnswersAsAdc(player, matchupInfos);
                    break;
                case (int)roleList.Support:
                    listMatchup = GetEstimatedAnswersAsSupport(player, matchupInfos);
                    break;
                default:
                    break;
            }

            return listMatchup;
        }

        private IEnumerable<Matchup> GetEstimatedAnswersAsAdc(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            // les 3 param
            if (matchupInfos.EnnemyAdc != Guid.Empty && matchupInfos.EnnemySupport != Guid.Empty && matchupInfos.AllySupport != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnnemySupport);
            }
            //sans le support allié
            else if (matchupInfos.EnnemyAdc != Guid.Empty && matchupInfos.EnnemySupport != Guid.Empty && matchupInfos.AllySupport == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnnemySupport);
            }
            //sans le support ennemy
            else if (matchupInfos.EnnemyAdc != Guid.Empty && matchupInfos.EnnemySupport == Guid.Empty && matchupInfos.AllySupport != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            // sans le support allié et support ennemy
            else if (matchupInfos.EnnemyAdc != Guid.Empty && matchupInfos.EnnemySupport == Guid.Empty && matchupInfos.AllySupport == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }

            return listMatchup;
        }

        private IEnumerable<Matchup> GetEstimatedAnswersAsSupport(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            // les 3 param
            if (matchupInfos.EnnemySupport != Guid.Empty && matchupInfos.EnnemyAdc != Guid.Empty && matchupInfos.AllyAdc != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnnemySupport);
            }
            //sans l'adc allié
            else if (matchupInfos.EnnemySupport != Guid.Empty && matchupInfos.EnnemyAdc != Guid.Empty && matchupInfos.AllyAdc == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnnemySupport);
            }
            //sans l'adc ennemy
            else if (matchupInfos.EnnemySupport != Guid.Empty && matchupInfos.EnnemyAdc == Guid.Empty && matchupInfos.AllyAdc != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnnemySupport);
            }
            // sans l'adc allié et l'adc ennemy
            else if (matchupInfos.EnnemySupport != Guid.Empty && matchupInfos.EnnemyAdc != Guid.Empty && matchupInfos.AllyAdc != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnnemySupport);
            }

            return listMatchup;
        }

        private IEnumerable<Matchup> GetEstimatedAnswersAsMidlaner(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            // les 3 param
            if (matchupInfos.EnnemyMid != Guid.Empty && matchupInfos.EnnemyJungle != Guid.Empty && matchupInfos.AllyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnnemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnnemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler allié
            else if (matchupInfos.EnnemyMid != Guid.Empty && matchupInfos.EnnemyJungle != Guid.Empty && matchupInfos.AllyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnnemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnnemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler ennemy
            else if (matchupInfos.EnnemyMid != Guid.Empty && matchupInfos.EnnemyJungle == Guid.Empty && matchupInfos.AllyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnnemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            // sans le jungler allié et ennemy
            else if (matchupInfos.EnnemyMid != Guid.Empty && matchupInfos.EnnemyJungle == Guid.Empty && matchupInfos.AllyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnnemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }

            return listMatchup;
        }

        private IEnumerable<Matchup> GetEstimatedAnswersAsToplaner(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            // les 3 param
            if (matchupInfos.EnnemyTop != Guid.Empty && matchupInfos.EnnemyJungle != Guid.Empty && matchupInfos.AllyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnnemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnnemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler allié
            else if (matchupInfos.EnnemyTop != Guid.Empty && matchupInfos.EnnemyJungle != Guid.Empty && matchupInfos.AllyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnnemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnnemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler ennemy
            else if (matchupInfos.EnnemyTop != Guid.Empty && matchupInfos.EnnemyJungle == Guid.Empty && matchupInfos.AllyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnnemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            // sans le jungler allié et ennemy
            else if (matchupInfos.EnnemyTop != Guid.Empty && matchupInfos.EnnemyJungle == Guid.Empty && matchupInfos.AllyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnnemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }

            return listMatchup;
        }

        private MatchupInfos CreateAutomaticMatchupInfo(MatchupInfos matchupInfos, List<Guid> listMatchupResponse)
        {
            MatchupInfos automaticMatchupInfo = new MatchupInfos();
            automaticMatchupInfo = matchupInfos;
            automaticMatchupInfo.answers = new List<MatchupAnswer>();

            foreach (Guid matchupResponseId in listMatchupResponse)
            {
                var matchupResponses = db.MatchupResponses.Where(x => x.MatchupResponseId == matchupResponseId).ToList();
                foreach (var matchupResponse in matchupResponses)
                {
                    var matchupAnswer = new MatchupAnswer();
                    matchupAnswer.ChampionId = matchupResponse.ChampionId;
                    automaticMatchupInfo.answers.Add(matchupAnswer);
                }
                automaticMatchupInfo.answers = automaticMatchupInfo.answers.OrderBy(x => x.ChampionName).ToList();
            }
            return automaticMatchupInfo;
        }

        public List<Champion> ComputeAnswerWithDraftInfos(Tuple<List<MatchupInfos>, long, MatchupInfos> listMatchupInfos, DraftInfos draftInfos, Guid playerId)
        {
            var listChampionIdBans = new List<Guid>();

            listChampionIdBans.Add(draftInfos.AllyBan1);
            listChampionIdBans.Add(draftInfos.AllyBan2);
            listChampionIdBans.Add(draftInfos.AllyBan3);
            listChampionIdBans.Add(draftInfos.AllyBan4);
            listChampionIdBans.Add(draftInfos.AllyBan5);

            listChampionIdBans.Add(draftInfos.EnnemyBan1);
            listChampionIdBans.Add(draftInfos.EnnemyBan2);
            listChampionIdBans.Add(draftInfos.EnnemyBan3);
            listChampionIdBans.Add(draftInfos.EnnemyBan4);
            listChampionIdBans.Add(draftInfos.EnnemyBan5);

            var listChampion = new HashSet<Champion>(new ChampionComparer());
            if (listMatchupInfos.Item1?.Count() > 0)
            {
                foreach (var matchupinfos in listMatchupInfos.Item1)
                {
                    foreach (var answer in matchupinfos.answers)
                    {
                        if (!listChampionIdBans.Contains(answer.ChampionId))
                            listChampion.Add(new Champion()
                            {
                                ChampionId = answer.ChampionId,
                                ChampionName = answer.ChampionName,
                                //ChampionTags = db.ChampionTags.Where(x => x.PlayerId == playerId && x.ChampionId == answer.ChampionId).ToList()
                            });
                    }
                }
            }
            if (listMatchupInfos.Item3.answers != null)
            {
                foreach (var answer in listMatchupInfos.Item3.answers)
                {
                    if (!listChampionIdBans.Contains(answer.ChampionId))
                        listChampion.Add(new Champion()
                        {
                            ChampionId = answer.ChampionId,
                            ChampionName = answer.ChampionName,
                            //ChampionTags = db.ChampionTags.Where(x => x.PlayerId == playerId && x.ChampionId == answer.ChampionId).ToList()
                        });
                }
            }
            if(listChampion.Count == 0)
            {
                foreach(var item in db.ChampionPools.Where(x => x.PlayerId == playerId).ToList())
                {
                    if (!listChampionIdBans.Contains(item.ChampionId))
                        listChampion.Add(new Champion()
                        {
                            ChampionId = item.ChampionId,
                            ChampionName = ((List<Champion>)System.Web.HttpContext.Current.Session["GlobalChampions"]).Where(x => x.ChampionId == item.ChampionId).FirstOrDefault()?.ChampionName
                            //ChampionTags = db.ChampionTags.Where(x => x.PlayerId == playerId && x.ChampionId == answer.ChampionId).ToList()
                        });
                }
            }

            return listChampion.OrderBy(x =>x.ChampionName).ToList();
        }
    }
}