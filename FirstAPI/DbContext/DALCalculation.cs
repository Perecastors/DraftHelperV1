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
            listMatchup = listMatchup.Where(x => x.AllyTop == matchupInfos.allyTop);
            listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.allyJungle);
            listMatchup = listMatchup.Where(x => x.AllyMid == matchupInfos.allyMid);
            listMatchup = listMatchup.Where(x => x.AllyAdc == matchupInfos.allyAdc);
            listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.allySupport);
            listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.ennemyTop);
            listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.ennemyJungle);
            listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.ennemyMid);
            listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc);
            listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.ennemySupport);

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
            if (matchupInfos.allyTop != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.AllyTop == matchupInfos.allyTop)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.allyJungle != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.AllyJungle == matchupInfos.allyJungle)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.allyMid != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.AllyMid == matchupInfos.allyMid)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.allyAdc != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.AllyAdc == matchupInfos.allyAdc)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.allySupport != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.AllySupport == matchupInfos.allySupport)).ToList();
                hasFilter = true;
            }
            //ENNEMI
            if (matchupInfos.ennemyTop != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyTop == matchupInfos.ennemyTop)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.ennemyJungle != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyJungle == matchupInfos.ennemyJungle)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.ennemyMid != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyMid == matchupInfos.ennemyMid)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.ennemyAdc != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.ennemySupport != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemySupport == matchupInfos.ennemySupport)).ToList();
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
                if (matchupInfos.allyTop != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyTop == matchupInfos.allyTop && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList).ToList();
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyTop == matchupInfos.allyTop && x.PlayerId == matchupInfos.playerId).ToList();
                    }
                    hasFilter = true;
                }
                if (matchupInfos.allyJungle != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyJungle == matchupInfos.allyJungle && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList).ToList();
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyJungle == matchupInfos.allyJungle && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.allyMid != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyMid == matchupInfos.allyMid && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyMid == matchupInfos.allyMid && x.PlayerId == matchupInfos.playerId);

                    }
                    hasFilter = true;
                }
                if (matchupInfos.allyAdc != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyAdc == matchupInfos.allyAdc && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyAdc == matchupInfos.allyAdc && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.allySupport != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllySupport == matchupInfos.allySupport && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllySupport == matchupInfos.allySupport && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                //ENNEMI
                if (matchupInfos.ennemyTop != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyTop == matchupInfos.ennemyTop && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyTop == matchupInfos.ennemyTop && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.ennemyJungle != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyJungle == matchupInfos.ennemyJungle && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyJungle == matchupInfos.ennemyJungle && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.ennemyMid != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyMid == matchupInfos.ennemyMid && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyMid == matchupInfos.ennemyMid && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.ennemyAdc != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc && x.PlayerId == matchupInfos.playerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.ennemySupport != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemySupport == matchupInfos.ennemySupport && x.PlayerId == matchupInfos.playerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemySupport == matchupInfos.ennemySupport && x.PlayerId == matchupInfos.playerId);
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
            if (matchupInfos.ennemyAdc != Guid.Empty && matchupInfos.ennemySupport != Guid.Empty && matchupInfos.allySupport != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.allySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.ennemySupport);
            }
            //sans le support allié
            else if (matchupInfos.ennemyAdc != Guid.Empty && matchupInfos.ennemySupport != Guid.Empty && matchupInfos.allySupport == Guid.Empty)
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
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.ennemySupport);
            }
            //sans le support ennemy
            else if (matchupInfos.ennemyAdc != Guid.Empty && matchupInfos.ennemySupport == Guid.Empty && matchupInfos.allySupport != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.allySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            // sans le support allié et support ennemy
            else if (matchupInfos.ennemyAdc != Guid.Empty && matchupInfos.ennemySupport == Guid.Empty && matchupInfos.allySupport == Guid.Empty)
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
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }

            return listMatchup;
        }

        private IEnumerable<Matchup> GetEstimatedAnswersAsSupport(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            // les 3 param
            if (matchupInfos.ennemySupport != Guid.Empty && matchupInfos.ennemyAdc != Guid.Empty && matchupInfos.allyAdc != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.allySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.ennemySupport);
            }
            //sans l'adc allié
            else if (matchupInfos.ennemySupport != Guid.Empty && matchupInfos.ennemyAdc != Guid.Empty && matchupInfos.allyAdc == Guid.Empty)
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
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.ennemySupport);
            }
            //sans l'adc ennemy
            else if (matchupInfos.ennemySupport != Guid.Empty && matchupInfos.ennemyAdc == Guid.Empty && matchupInfos.allyAdc != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.allySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.ennemySupport);
            }
            // sans l'adc allié et l'adc ennemy
            else if (matchupInfos.ennemySupport != Guid.Empty && matchupInfos.ennemyAdc != Guid.Empty && matchupInfos.allyAdc != Guid.Empty)
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
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.ennemySupport);
            }

            return listMatchup;
        }

        private IEnumerable<Matchup> GetEstimatedAnswersAsMidlaner(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            // les 3 param
            if (matchupInfos.ennemyMid != Guid.Empty && matchupInfos.ennemyJungle != Guid.Empty && matchupInfos.allyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.allyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.ennemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.ennemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler allié
            else if (matchupInfos.ennemyMid != Guid.Empty && matchupInfos.ennemyJungle != Guid.Empty && matchupInfos.allyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.ennemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.ennemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler ennemy
            else if (matchupInfos.ennemyMid != Guid.Empty && matchupInfos.ennemyJungle == Guid.Empty && matchupInfos.allyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.allyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.ennemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            // sans le jungler allié et ennemy
            else if (matchupInfos.ennemyTop != Guid.Empty && matchupInfos.ennemyJungle == Guid.Empty && matchupInfos.allyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.ennemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }

            return listMatchup;
        }

        private IEnumerable<Matchup> GetEstimatedAnswersAsToplaner(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            // les 3 param
            if (matchupInfos.ennemyTop != Guid.Empty && matchupInfos.ennemyJungle != Guid.Empty && matchupInfos.allyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.allyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.ennemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.ennemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler allié
            else if (matchupInfos.ennemyTop != Guid.Empty && matchupInfos.ennemyJungle != Guid.Empty && matchupInfos.allyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.ennemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.ennemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler ennemy
            else if (matchupInfos.ennemyTop != Guid.Empty && matchupInfos.ennemyJungle == Guid.Empty && matchupInfos.allyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.allyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.ennemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            // sans le jungler allié et ennemy
            else if (matchupInfos.ennemyTop != Guid.Empty && matchupInfos.ennemyJungle == Guid.Empty && matchupInfos.allyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.playerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.ennemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }

            return listMatchup;
        }

        private MatchupInfos CreateAutomaticMatchupInfo(MatchupInfos matchupInfos, List<Guid> listMatchupResponse)
        {
            foreach (Guid matchupResponseId in listMatchupResponse)
            {
                var matchupResponses = db.MatchupResponses.Where(x => x.MatchupResponseId == matchupResponseId).ToList();
                foreach (var matchupResponse in matchupResponses)
                {
                    var matchupAnswer = new MatchupAnswer();
                    matchupAnswer.ChampionId = matchupResponse.ChampionId;
                    if (matchupInfos.answers == null)
                    {
                        matchupInfos.answers = new List<MatchupAnswer>();
                    }
                    if (!matchupInfos.answers.Where(x => x.ChampionId == matchupResponse.ChampionId).Any())
                    {
                        matchupInfos.answers.Add(matchupAnswer);
                    }
                }
            }
            return matchupInfos;
        }
    }
}