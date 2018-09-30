using FirstAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web;

namespace FirstAPI.DbContext
{
    public class DALMatchup
    {
        Database1Entities db;
        public DALMatchup()
        {
            db = new Database1Entities();
            if (ConfigurationManager.AppSettings["Environment"].ToLower() == "prod")
                db = new Database1Entities("name=Database2Entities");
        }

        public MatchupInfos getMatchupByMatchupId(Guid matchupId)
        {
            MatchupInfos matchupInfos = new MatchupInfos();

            var matchup = db.Matchups.Where(x => x.MatchupId == matchupId).FirstOrDefault();
            var comments = db.MatchupComments.Where(x => x.PlayerId == matchup.PlayerId && x.MatchupId == matchupId).OrderByDescending(x => x.CreationDate).ToList();
            matchupInfos = ConvertMatchupsToMatchupInfos(new List<Matchup>() { matchup }, comments).FirstOrDefault();
            return matchupInfos;
        }

        public Tuple<List<MatchupInfos>, long,MatchupInfos> getAllMatchupByParams(Guid playerId, MatchupInfos matchupInfos)
        {
            List<MatchupInfos> listMatchupInfos = new List<MatchupInfos>();


            var listMatchup = db.Matchups.Where(x => x.PlayerId == playerId);
            bool hasFilter = false;
            //ALLY
            if (matchupInfos.allyTop != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.AllyTop == matchupInfos.allyTop);
                hasFilter = true;
            }
            if (matchupInfos.allyJungle != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.allyJungle);
                hasFilter = true;
            }
            if (matchupInfos.allyMid != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.AllyMid == matchupInfos.allyMid);
                hasFilter = true;
            }
            if (matchupInfos.allyAdc != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.AllyAdc == matchupInfos.allyAdc);
                hasFilter = true;
            }
            if (matchupInfos.allySupport != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.allySupport);
                hasFilter = true;
            }
            //ENNEMI
            if (matchupInfos.ennemyTop != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.ennemyTop);
                hasFilter = true;
            }
            if (matchupInfos.ennemyJungle != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.ennemyJungle);
                hasFilter = true;
            }
            if (matchupInfos.ennemyMid != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.ennemyMid);
                hasFilter = true;
            }
            if (matchupInfos.ennemyAdc != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.ennemyAdc);
                hasFilter = true;
            }
            if (matchupInfos.ennemySupport != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.ennemySupport);
                hasFilter = true;
            }

            bool isPerfectMatch = isPerfectMatchExist(matchupInfos, playerId);

            long time;
            var automaticMatchup = new MatchupInfos();
            if (true)
            {
                var listMatchupComment = db.MatchupComments.Where(x => x.PlayerId == playerId).ToList();
                Stopwatch t0 = new Stopwatch();
                t0.Start();
                var listMatchups = listMatchup.OrderByDescending(x => x.CreationDate).Take(20).ToList();
                time = t0.ElapsedMilliseconds;
                listMatchupInfos = ConvertMatchupsToMatchupInfos(listMatchups, listMatchupComment);
                if(!isPerfectMatch)
                {
                    matchupInfos.playerId = playerId;
                    automaticMatchup = GetEstimatedAnswersByMatchupParam(matchupInfos);
                }
                var t2 = t0.ElapsedMilliseconds;
            }

            return new Tuple<List<MatchupInfos>, long, MatchupInfos>(listMatchupInfos, time, automaticMatchup);
        }

        private bool isPerfectMatchExist(MatchupInfos matchupInfos, Guid playerId)
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

            if(listMatchup.Count() > 0)
                return true;

            return false;
        }

        public MatchupInfos GetEstimatedAnswersByMatchupParam(MatchupInfos matchupInfos)
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

        public MatchupInfos GetEstimatedAnswersByMatchupParam2(MatchupInfos matchupInfos)
        {
            IEnumerable<Matchup> listMatchup = Enumerable.Empty<Matchup>().AsQueryable();

            bool hasFilter = false;
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

            var automaticMatchupInfos = new MatchupInfos();
            if (hasFilter)
            {
                //recuperer la liste das matchupResponsesId pour recuperer la liste des champions
                var listMatchupResponse = listMatchup.Select(x => x.MatchupResponseId).ToList();
                automaticMatchupInfos = CreateAutomaticMatchupInfo(matchupInfos, listMatchupResponse);
            }

            return automaticMatchupInfos;
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

        public Guid createNewMatchup(Guid playerId, MatchupInfos matchupInfos)
        {
            Matchup matchup = buildMatchupEntity(matchupInfos);
            List<MatchupRespons> matchupResponses = buildMatchupResponsEntity(matchupInfos, matchup.MatchupResponseId);
            matchup.PlayerId = playerId;
            using (TransactionScope scope = new TransactionScope())
            {
                db.Matchups.Add(matchup);
                db.MatchupResponses.AddRange(matchupResponses);
                db.SaveChanges();
                scope.Complete();
                return matchup.MatchupId;
            }
        }

        public List<MatchupInfos> ConvertMatchupsToMatchupInfos(List<Matchup> listMatchup, List<MatchupComment> listMatchupComment)

        {
            List<MatchupInfos> matchupInfos = new List<MatchupInfos>();

            if (listMatchup?.Count > 0)
            {
                foreach (var matchup in listMatchup)
                {
                    MatchupInfos matchupInfo = new MatchupInfos();
                    matchupInfo.matchupId = matchup.MatchupId;
                    matchupInfo.matchupResponseId = matchup.MatchupResponseId;
                    matchupInfo.allyTop = matchup.AllyTop.Value;
                    matchupInfo.allyJungle = matchup.AllyJungle.Value;
                    matchupInfo.allyMid = matchup.AllyMid.Value;
                    matchupInfo.allyAdc = matchup.AllyAdc.Value;
                    matchupInfo.allySupport = matchup.AllySupport.Value;

                    matchupInfo.ennemyTop = matchup.EnemyTop.Value;
                    matchupInfo.ennemyJungle = matchup.EnemyJungle.Value;
                    matchupInfo.ennemyMid = matchup.EnemyMid.Value;
                    matchupInfo.ennemyAdc = matchup.EnemyAdc.Value;
                    matchupInfo.ennemySupport = matchup.EnemySupport.Value;
                    var matchupResponse = db.MatchupResponses.Where(x => x.MatchupResponseId == matchup.MatchupResponseId);
                    matchupInfo.answers = new List<MatchupAnswer>();
                    foreach (var response in matchupResponse)
                    {
                        var matchupAnswer = new MatchupAnswer();
                        matchupAnswer.MatchupCommentId = response.MatchupResponseId;
                        matchupAnswer.ChampionId = response.ChampionId;
                        matchupAnswer.Comments = listMatchupComment.Where(x => x.MatchupId == matchup.MatchupId && x.ChampionId == response.ChampionId).OrderByDescending(x => x.CreationDate).FirstOrDefault()?.CommentText;
                        matchupInfo.answers.Add(matchupAnswer);
                    }
                    matchupInfos.Add(matchupInfo);
                }
            }
            return matchupInfos;

        }


        public int AddMatchup(MatchupInfos matchupInfos)
        {
            int result = -1;
            Matchup matchup = buildMatchupEntity(matchupInfos);
            List<MatchupRespons> matchupResponses = buildMatchupResponsEntity(matchupInfos, matchup.MatchupResponseId);
            List<MatchupComment> matchupComments = buildMatchupCommentsEntity(matchupInfos, matchup);
            if (matchup.MatchupId != Guid.Empty)
                result = saveMatchup(matchup, matchupResponses, matchupComments);

            return result;
        }

        public int saveMatchup(Matchup matchup, List<MatchupRespons> matchupResponses, List<MatchupComment> matchupComments)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var existingMatchup = db.Matchups.SingleOrDefault(x => x.MatchupId == matchup.MatchupId);
                if (existingMatchup == null)
                {
                    db.Matchups.Add(matchup);
                }
                else
                {
                    db.Entry(existingMatchup).CurrentValues.SetValues(matchup);
                }
                db.MatchupResponses.AddRange(matchupResponses);
                db.MatchupComments.AddRange(matchupComments);
                int result = db.SaveChanges();
                scope.Complete();
                return result;
            }
        }

        public int deleteMatchup(Guid matchupId, Guid matchupResponseId)
        {
            var matchup = db.Matchups.Where(x => x.MatchupId == matchupId).FirstOrDefault();
            using (TransactionScope scope = new TransactionScope())
            {
                //delete comments
                db.MatchupComments.RemoveRange(db.MatchupComments.Where(x => x.MatchupId == matchupId));
                //delete responses
                db.MatchupResponses.RemoveRange(db.MatchupResponses.Where(x => x.MatchupResponseId == matchupResponseId));
                db.Matchups.Remove(matchup);
                int result = db.SaveChanges();
                scope.Complete();
                return result;
            }
        }

        public Matchup buildMatchupEntity(MatchupInfos matchupInfos)
        {
            Matchup matchup = new Matchup();

            matchup.AllyTop = matchupInfos.allyTop;
            matchup.AllyJungle = matchupInfos.allyJungle;
            matchup.AllyMid = matchupInfos.allyMid;
            matchup.AllyAdc = matchupInfos.allyAdc;
            matchup.AllySupport = matchupInfos.allySupport;

            matchup.EnemyTop = matchupInfos.ennemyTop;
            matchup.EnemyJungle = matchupInfos.ennemyJungle;
            matchup.EnemyMid = matchupInfos.ennemyMid;
            matchup.EnemyAdc = matchupInfos.ennemyAdc;
            matchup.EnemySupport = matchupInfos.ennemySupport;

            if (matchupInfos.matchupId == Guid.Empty)
                matchup.MatchupId = Guid.NewGuid();
            else
                matchup.MatchupId = matchupInfos.matchupId;

            matchup.PlayerId = matchupInfos.playerId;
            if (matchupInfos.matchupResponseId == Guid.Empty)
                matchup.MatchupResponseId = Guid.NewGuid();
            else
                matchup.MatchupResponseId = matchupInfos.matchupResponseId;
            matchup.CreationDate = DateTime.Now;
            return matchup;
        }

        public List<MatchupRespons> buildMatchupResponsEntity(MatchupInfos matchupInfos, Guid matchupResponseId)
        {
            List<MatchupRespons> matchupResponses = new List<MatchupRespons>();
            if (matchupInfos.answers?.Count > 0)
            {
                foreach (var answer in matchupInfos.answers)
                {
                    MatchupRespons matchupRespons = new MatchupRespons();
                    matchupRespons.ChampionId = answer.ChampionId;
                    matchupRespons.MatchupResponseId = matchupResponseId;
                    matchupRespons.CreationDate = DateTime.Now;
                    matchupResponses.Add(matchupRespons);
                }
            }

            return matchupResponses;
        }

        public List<MatchupComment> buildMatchupCommentsEntity(MatchupInfos matchupInfos, Matchup matchup)
        {
            List<MatchupComment> matchupComments = new List<MatchupComment>();
            if (matchupInfos.answers?.Count > 0)
            {
                foreach (var comment in matchupInfos.answers)
                {
                    //var existingmatchupComment = db.MatchupComments.Where(x => x.MatchupId == matchupInfos.matchupId && x.ChampionId == comment.Id);
                    MatchupComment matchupComment = new MatchupComment();
                    matchupComment.ChampionId = comment.ChampionId;
                    matchupComment.CommentText = comment.Comments;
                    matchupComment.PlayerId = matchup.PlayerId;
                    matchupComment.MatchupCommentId = Guid.NewGuid();
                    matchupComment.CreationDate = DateTime.Now;
                    matchupComment.MatchupId = matchup.MatchupId;
                    matchupComments.Add(matchupComment);
                }
            }


            return matchupComments;
        }

    }
}