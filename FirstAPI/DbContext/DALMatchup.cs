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

        public List<Champion> getAnswersForDraftByPlayer(Guid playerId, DraftInfos draftInfos)
        {
            Tuple<List<MatchupInfos>, long, MatchupInfos> listMatchupInfos;
            listMatchupInfos = getAllMatchupByParams(playerId, draftInfos);
            var listChampions = new DALCalculation().ComputeAnswerWithDraftInfos(listMatchupInfos,draftInfos, playerId);
            return listChampions.OrderBy(x => x.ChampionName).ToList();
        }

        

        public Tuple<List<MatchupInfos>, long,MatchupInfos> getAllMatchupByParams(Guid playerId, MatchupInfos matchupInfos)
        {
            List<MatchupInfos> listMatchupInfos = new List<MatchupInfos>();
            var dalCalculation = new DALCalculation();

            var listMatchup = db.Matchups.Where(x => x.PlayerId == playerId);
            bool hasFilter = false;
            //ALLY
            if (matchupInfos.AllyTop != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.AllyTop == matchupInfos.AllyTop);
                hasFilter = true;
            }
            if (matchupInfos.AllyJungle != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.AllyJungle == matchupInfos.AllyJungle);
                hasFilter = true;
            }
            if (matchupInfos.AllyMid != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.AllyMid == matchupInfos.AllyMid);
                hasFilter = true;
            }
            if (matchupInfos.AllyAdc != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.AllyAdc == matchupInfos.AllyAdc);
                hasFilter = true;
            }
            if (matchupInfos.AllySupport != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.AllySupport == matchupInfos.AllySupport);
                hasFilter = true;
            }
            //ENNEMI
            if (matchupInfos.EnnemyTop != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnnemyTop);
                hasFilter = true;
            }
            if (matchupInfos.EnnemyJungle != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnnemyJungle);
                hasFilter = true;
            }
            if (matchupInfos.EnnemyMid != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnnemyMid);
                hasFilter = true;
            }
            if (matchupInfos.EnnemyAdc != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnnemyAdc);
                hasFilter = true;
            }
            if (matchupInfos.EnnemySupport != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnnemySupport);
                hasFilter = true;
            }

            bool isPerfectMatch = dalCalculation.isPerfectMatchExist(matchupInfos, playerId);

            long time;
            var automaticMatchup = new MatchupInfos();
            if (true)
            {
                var listMatchupComment = db.MatchupComments.Where(x => x.PlayerId == playerId).ToList();
                Stopwatch t0 = new Stopwatch();
                t0.Start();
                var listMatchups = listMatchup.OrderByDescending(x => x.CreationDate).Take(40).ToList();
                time = t0.ElapsedMilliseconds;
                listMatchupInfos = ConvertMatchupsToMatchupInfos(listMatchups, listMatchupComment);
                if(!isPerfectMatch)
                {
                    matchupInfos.playerId = playerId;
                    automaticMatchup = dalCalculation.GetEstimatedAnswersByMatchupParam(matchupInfos);
                }
                var t2 = t0.ElapsedMilliseconds;
            }

            return new Tuple<List<MatchupInfos>, long, MatchupInfos>(listMatchupInfos, time, automaticMatchup);
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
                    matchupInfo.AllyTop = matchup.AllyTop.Value;
                    matchupInfo.AllyJungle = matchup.AllyJungle.Value;
                    matchupInfo.AllyMid = matchup.AllyMid.Value;
                    matchupInfo.AllyAdc = matchup.AllyAdc.Value;
                    matchupInfo.AllySupport = matchup.AllySupport.Value;

                    matchupInfo.EnnemyTop = matchup.EnemyTop.Value;
                    matchupInfo.EnnemyJungle = matchup.EnemyJungle.Value;
                    matchupInfo.EnnemyMid = matchup.EnemyMid.Value;
                    matchupInfo.EnnemyAdc = matchup.EnemyAdc.Value;
                    matchupInfo.EnnemySupport = matchup.EnemySupport.Value;
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

            matchup.AllyTop = matchupInfos.AllyTop;
            matchup.AllyJungle = matchupInfos.AllyJungle;
            matchup.AllyMid = matchupInfos.AllyMid;
            matchup.AllyAdc = matchupInfos.AllyAdc;
            matchup.AllySupport = matchupInfos.AllySupport;

            matchup.EnemyTop = matchupInfos.EnnemyTop;
            matchup.EnemyJungle = matchupInfos.EnnemyJungle;
            matchup.EnemyMid = matchupInfos.EnnemyMid;
            matchup.EnemyAdc = matchupInfos.EnnemyAdc;
            matchup.EnemySupport = matchupInfos.EnnemySupport;

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