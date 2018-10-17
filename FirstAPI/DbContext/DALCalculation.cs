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



        public bool isPerfectMatchupExistWithMatchupInfo(MatchupInfos matchupInfos, Guid playerId)
        {
            var listMatchup = db.Matchups.Where(x => x.PlayerId == playerId);
            listMatchup = listMatchup.Where(x => x.AllyTop == matchupInfos.AllyTop);
            listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
            listMatchup = listMatchup.Where(x => x.AllyMid == matchupInfos.AllyMid);
            listMatchup = listMatchup.Where(x => x.AllyAdc == matchupInfos.AllyAdc);
            listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
            listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnemyTop);
            listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnemyJungle);
            listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnemyMid);
            listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc);
            listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnemySupport);
            if(matchupInfos.PatchVersion != null)
            {
                listMatchup = listMatchup.Where(x => x.PatchVersion == matchupInfos.PatchVersion);
            }
            if (listMatchup.Count() > 0)
                return true;

            return false;
        }

        public bool isPerfectMatchupExist(Matchup matchup)
        {
            var listMatchup = db.Matchups.Where(x => x.PlayerId == matchup.PlayerId);
            listMatchup = listMatchup.Where(x => x.AllyTop == matchup.AllyTop);
            listMatchup = listMatchup.Where(x => x.AllyJungle == matchup.AllyJungle);
            listMatchup = listMatchup.Where(x => x.AllyMid == matchup.AllyMid);
            listMatchup = listMatchup.Where(x => x.AllyAdc == matchup.AllyAdc);
            listMatchup = listMatchup.Where(x => x.AllySupport == matchup.AllySupport);
            listMatchup = listMatchup.Where(x => x.EnemyTop == matchup.EnemyTop);
            listMatchup = listMatchup.Where(x => x.EnemyJungle == matchup.EnemyJungle);
            listMatchup = listMatchup.Where(x => x.EnemyMid == matchup.EnemyMid);
            listMatchup = listMatchup.Where(x => x.EnemyAdc == matchup.EnemyAdc);
            listMatchup = listMatchup.Where(x => x.EnemySupport == matchup.EnemySupport);
            listMatchup = listMatchup.Where(x => x.PatchVersion == matchup.PatchVersion);
            if (listMatchup.Count() > 0)
                return true;

            return false;
        }

        public MatchupInfos GetEstimatedAnswersWithAllParam(MatchupInfos matchupInfos)
        {
            IQueryable<Matchup> listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
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
            if (matchupInfos.EnemyTop != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyTop == matchupInfos.EnemyTop)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.EnemyJungle != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnemyJungle)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.EnemyMid != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyMid == matchupInfos.EnemyMid)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.EnemyAdc != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc)).ToList();
                hasFilter = true;
            }
            if (matchupInfos.EnemySupport != Guid.Empty)
            {
                calcultedList = calcultedList.Union(listMatchup.Where(x => x.EnemySupport == matchupInfos.EnemySupport)).ToList();
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
            var player = new DAL().getPlayerById(matchupInfos.PlayerId);
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
                        var tempList = listMatchup.Where(x => x.AllyTop == matchupInfos.AllyTop && x.PlayerId == matchupInfos.PlayerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList).ToList();
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyTop == matchupInfos.AllyTop && x.PlayerId == matchupInfos.PlayerId).ToList();
                    }
                    hasFilter = true;
                }
                if (matchupInfos.AllyJungle != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle && x.PlayerId == matchupInfos.PlayerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList).ToList();
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyJungle == matchupInfos.AllyJungle && x.PlayerId == matchupInfos.PlayerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.AllyMid != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyMid == matchupInfos.AllyMid && x.PlayerId == matchupInfos.PlayerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyMid == matchupInfos.AllyMid && x.PlayerId == matchupInfos.PlayerId);

                    }
                    hasFilter = true;
                }
                if (matchupInfos.AllyAdc != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllyAdc == matchupInfos.AllyAdc && x.PlayerId == matchupInfos.PlayerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllyAdc == matchupInfos.AllyAdc && x.PlayerId == matchupInfos.PlayerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.AllySupport != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport && x.PlayerId == matchupInfos.PlayerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.AllySupport == matchupInfos.AllySupport && x.PlayerId == matchupInfos.PlayerId);
                    }
                    hasFilter = true;
                }
                //ENNEMI
                if (matchupInfos.EnemyTop != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnemyTop && x.PlayerId == matchupInfos.PlayerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyTop == matchupInfos.EnemyTop && x.PlayerId == matchupInfos.PlayerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.EnemyJungle != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnemyJungle && x.PlayerId == matchupInfos.PlayerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyJungle == matchupInfos.EnemyJungle && x.PlayerId == matchupInfos.PlayerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.EnemyMid != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnemyMid && x.PlayerId == matchupInfos.PlayerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyMid == matchupInfos.EnemyMid && x.PlayerId == matchupInfos.PlayerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.EnemyAdc != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc && x.PlayerId == matchupInfos.PlayerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc && x.PlayerId == matchupInfos.PlayerId);
                    }
                    hasFilter = true;
                }
                if (matchupInfos.EnemySupport != Guid.Empty)
                {
                    if (listMatchup.Count() > 0)
                    {
                        var tempList = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnemySupport && x.PlayerId == matchupInfos.PlayerId);
                        if (tempList.Count() > 0)
                        {
                            listMatchup = listMatchup.Intersect(tempList);
                        }
                    }
                    else
                    {
                        listMatchup = db.Matchups.Where(x => x.EnemySupport == matchupInfos.EnemySupport && x.PlayerId == matchupInfos.PlayerId);
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
            if (matchupInfos.EnemyAdc != Guid.Empty && matchupInfos.EnemySupport != Guid.Empty && matchupInfos.AllySupport != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnemySupport);
            }
            //sans le support allié
            else if (matchupInfos.EnemyAdc != Guid.Empty && matchupInfos.EnemySupport != Guid.Empty && matchupInfos.AllySupport == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnemySupport);
            }
            //sans le support Enemy
            else if (matchupInfos.EnemyAdc != Guid.Empty && matchupInfos.EnemySupport == Guid.Empty && matchupInfos.AllySupport != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            // sans le support allié et support Enemy
            else if (matchupInfos.EnemyAdc != Guid.Empty && matchupInfos.EnemySupport == Guid.Empty && matchupInfos.AllySupport == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }

            return listMatchup;
        }

        private IEnumerable<Matchup> GetEstimatedAnswersAsSupport(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            // les 3 param
            if (matchupInfos.EnemySupport != Guid.Empty && matchupInfos.EnemyAdc != Guid.Empty && matchupInfos.AllyAdc != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnemySupport);
            }
            //sans l'adc allié
            else if (matchupInfos.EnemySupport != Guid.Empty && matchupInfos.EnemyAdc != Guid.Empty && matchupInfos.AllyAdc == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnemySupport);
            }
            //sans l'adc Enemy
            else if (matchupInfos.EnemySupport != Guid.Empty && matchupInfos.EnemyAdc == Guid.Empty && matchupInfos.AllyAdc != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnemySupport);
            }
            // sans l'adc allié et l'adc Enemy
            else if (matchupInfos.EnemySupport != Guid.Empty && matchupInfos.EnemyAdc != Guid.Empty && matchupInfos.AllyAdc != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnemySupport);
            }

            return listMatchup;
        }

        private IEnumerable<Matchup> GetEstimatedAnswersAsMidlaner(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            // les 3 param
            if (matchupInfos.EnemyMid != Guid.Empty && matchupInfos.EnemyJungle != Guid.Empty && matchupInfos.AllyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler allié
            else if (matchupInfos.EnemyMid != Guid.Empty && matchupInfos.EnemyJungle != Guid.Empty && matchupInfos.AllyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler Enemy
            else if (matchupInfos.EnemyMid != Guid.Empty && matchupInfos.EnemyJungle == Guid.Empty && matchupInfos.AllyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            // sans le jungler allié et Enemy
            else if (matchupInfos.EnemyMid != Guid.Empty && matchupInfos.EnemyJungle == Guid.Empty && matchupInfos.AllyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnemyMid);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }

            return listMatchup;
        }

        private IEnumerable<Matchup> GetEstimatedAnswersAsToplaner(Player player, MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            // les 3 param
            if (matchupInfos.EnemyTop != Guid.Empty && matchupInfos.EnemyJungle != Guid.Empty && matchupInfos.AllyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler allié
            else if (matchupInfos.EnemyTop != Guid.Empty && matchupInfos.EnemyJungle != Guid.Empty && matchupInfos.AllyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnemyJungle);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            //sans le jungler Enemy
            else if (matchupInfos.EnemyTop != Guid.Empty && matchupInfos.EnemyJungle == Guid.Empty && matchupInfos.AllyJungle != Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnemyTop);
                listMatchup = listMatchup.Where(x => x.EnemyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemySupport == Guid.Empty);
            }
            // sans le jungler allié et Enemy
            else if (matchupInfos.EnemyTop != Guid.Empty && matchupInfos.EnemyJungle == Guid.Empty && matchupInfos.AllyJungle == Guid.Empty)
            {
                listMatchup = db.Matchups.Where(x => x.PlayerId == matchupInfos.PlayerId);
                listMatchup = listMatchup.Where(x => x.AllyTop == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyJungle == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyMid == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllyAdc == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.AllySupport == Guid.Empty);
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnemyTop);
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
            automaticMatchupInfo = matchupInfos.DuplicateMe(matchupInfos);
            automaticMatchupInfo.Answers = new List<MatchupAnswer>();

            foreach (Guid matchupResponseId in listMatchupResponse)
            {
                var matchupResponses = db.MatchupResponses.Where(x => x.MatchupResponseId == matchupResponseId).ToList();
                foreach (var matchupResponse in matchupResponses)
                {
                    var matchupAnswer = new MatchupAnswer();
                    matchupAnswer.ChampionId = matchupResponse.ChampionId;
                    automaticMatchupInfo.Answers.Add(matchupAnswer);
                }
                automaticMatchupInfo.Answers = automaticMatchupInfo.Answers.OrderBy(x => x.ChampionName).ToList();
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

            listChampionIdBans.Add(draftInfos.EnemyBan1);
            listChampionIdBans.Add(draftInfos.EnemyBan2);
            listChampionIdBans.Add(draftInfos.EnemyBan3);
            listChampionIdBans.Add(draftInfos.EnemyBan4);
            listChampionIdBans.Add(draftInfos.EnemyBan5);

            var listChampion = new HashSet<Champion>(new ChampionComparer());
            if (listMatchupInfos.Item1?.Count() > 0)
            {
                foreach (var matchupinfos in listMatchupInfos.Item1)
                {
                    foreach (var answer in matchupinfos.Answers)
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
            if (listMatchupInfos.Item3.Answers != null)
            {
                foreach (var answer in listMatchupInfos.Item3.Answers)
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