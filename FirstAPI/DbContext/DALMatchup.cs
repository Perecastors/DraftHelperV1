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
            var listChampions = new DALCalculation().ComputeAnswerWithDraftInfos(listMatchupInfos, draftInfos, playerId);
            return listChampions.OrderBy(x => x.ChampionName).ToList();
        }



        public Tuple<List<MatchupInfos>, long, MatchupInfos> getAllMatchupByParams(Guid playerId, MatchupInfos matchupInfos)
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
            if (matchupInfos.EnemyTop != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyTop == matchupInfos.EnemyTop);
                hasFilter = true;
            }
            if (matchupInfos.EnemyJungle != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyJungle == matchupInfos.EnemyJungle);
                hasFilter = true;
            }
            if (matchupInfos.EnemyMid != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyMid == matchupInfos.EnemyMid);
                hasFilter = true;
            }
            if (matchupInfos.EnemyAdc != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemyAdc == matchupInfos.EnemyAdc);
                hasFilter = true;
            }
            if (matchupInfos.EnemySupport != Guid.Empty)
            {
                listMatchup = listMatchup.Where(x => x.EnemySupport == matchupInfos.EnemySupport);
                hasFilter = true;
            }

            bool isPerfectMatch = dalCalculation.isPerfectMatchupExistWithMatchupInfo(matchupInfos, playerId);

            long time;
            var automaticMatchup = new MatchupInfos();
            if (true)
            {
                var listMatchupComment = db.MatchupComments.Where(x => x.PlayerId == playerId).ToList();
                Stopwatch t0 = new Stopwatch();
                t0.Start();
                var listMatchups = listMatchup.OrderByDescending(x=> x.PatchVersion).ThenBy(x => x.CreationDate).Take(200).ToList();
                time = t0.ElapsedMilliseconds;
                listMatchupInfos = ConvertMatchupsToMatchupInfos(listMatchups, listMatchupComment);
                if (!isPerfectMatch)
                {
                    matchupInfos.PlayerId = playerId;
                    automaticMatchup = dalCalculation.GetEstimatedAnswersByMatchupParam(matchupInfos);
                }
                var t2 = t0.ElapsedMilliseconds;
            }

            return new Tuple<List<MatchupInfos>, long, MatchupInfos>(listMatchupInfos, time, automaticMatchup);
        }

        public Tuple<List<MatchupInfos>, long> getAllMatchupByBestAnswer(Guid playerId, Guid championId)
        {
            List<MatchupInfos> listMatchupInfos = new List<MatchupInfos>();
            bool exist = db.Champions.Where(x => x.ChampionId == championId).Any();
            long time;
            Stopwatch t0 = new Stopwatch();
            t0.Start();
            if (exist)
            {
                var listMatchupResponses = db.MatchupResponses.Where(x => x.ChampionId == championId);
                var listMatchups = (from m in db.Matchups.Where(x => x.PlayerId == playerId)
                                    join mr in listMatchupResponses on m.MatchupResponseId equals mr.MatchupResponseId
                                    select m).OrderByDescending(x => x.PatchVersion).ThenBy(x => x.CreationDate).ToList();

                listMatchupInfos = ConvertMatchupsToMatchupInfos(listMatchups, new List<MatchupComment>());
            }
            time = t0.ElapsedMilliseconds;
            return new Tuple<List<MatchupInfos>, long>(listMatchupInfos, time);
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
                    matchupInfo.MatchupId = matchup.MatchupId;
                    matchupInfo.MatchupResponseId = matchup.MatchupResponseId;
                    matchupInfo.AllyTop = matchup.AllyTop.Value;
                    matchupInfo.AllyJungle = matchup.AllyJungle.Value;
                    matchupInfo.AllyMid = matchup.AllyMid.Value;
                    matchupInfo.AllyAdc = matchup.AllyAdc.Value;
                    matchupInfo.AllySupport = matchup.AllySupport.Value;

                    matchupInfo.EnemyTop = matchup.EnemyTop.Value;
                    matchupInfo.EnemyJungle = matchup.EnemyJungle.Value;
                    matchupInfo.EnemyMid = matchup.EnemyMid.Value;
                    matchupInfo.EnemyAdc = matchup.EnemyAdc.Value;
                    matchupInfo.EnemySupport = matchup.EnemySupport.Value;
                    matchupInfo.PatchVersion = matchup.PatchVersion;
                    var matchupResponse = db.MatchupResponses.Where(x => x.MatchupResponseId == matchup.MatchupResponseId);
                    matchupInfo.Answers = new List<MatchupAnswer>();
                    foreach (var response in matchupResponse)
                    {
                        var matchupAnswer = new MatchupAnswer();
                        matchupAnswer.MatchupCommentId = response.MatchupResponseId;
                        matchupAnswer.ChampionId = response.ChampionId;
                        matchupAnswer.Comments = listMatchupComment.Where(x => x.MatchupId == matchup.MatchupId && x.ChampionId == response.ChampionId).OrderByDescending(x => x.CreationDate).FirstOrDefault()?.CommentText;
                        matchupInfo.Answers.Add(matchupAnswer);
                    }
                    matchupInfo.Answers = matchupInfo.Answers.OrderBy(x => x.ChampionName).ToList();
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
                else if (existingMatchup != null && matchup.PatchVersion != ConfigurationManager.AppSettings["PatchVersion"])
                {
                    int result2 = SaveAsDuplicateMatchupWithActualPatch(existingMatchup, matchupResponses, matchupComments);
                    if (result2 > 0)
                        scope.Complete();
                    return result2;
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

            matchup.EnemyTop = matchupInfos.EnemyTop;
            matchup.EnemyJungle = matchupInfos.EnemyJungle;
            matchup.EnemyMid = matchupInfos.EnemyMid;
            matchup.EnemyAdc = matchupInfos.EnemyAdc;
            matchup.EnemySupport = matchupInfos.EnemySupport;

            if (matchupInfos.MatchupId == Guid.Empty)
                matchup.MatchupId = Guid.NewGuid();
            else
                matchup.MatchupId = matchupInfos.MatchupId;

            matchup.PlayerId = matchupInfos.PlayerId;
            if (matchupInfos.MatchupResponseId == Guid.Empty)
                matchup.MatchupResponseId = Guid.NewGuid();
            else
                matchup.MatchupResponseId = matchupInfos.MatchupResponseId;
            matchup.CreationDate = DateTime.Now;
            matchup.PatchVersion = matchupInfos.PatchVersion;
            if (String.IsNullOrEmpty(matchupInfos.PatchVersion))
            {
                matchup.PatchVersion = ConfigurationManager.AppSettings["PatchVersion"];
            }

            return matchup;
        }

        public List<MatchupRespons> buildMatchupResponsEntity(MatchupInfos matchupInfos, Guid matchupResponseId)
        {
            List<MatchupRespons> matchupResponses = new List<MatchupRespons>();
            if (matchupInfos.Answers?.Count > 0)
            {
                foreach (var answer in matchupInfos.Answers)
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
            if (matchupInfos.Answers?.Count > 0)
            {
                foreach (var comment in matchupInfos.Answers)
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

        public int SaveAsDuplicateMatchupWithActualPatch(Matchup matchup, List<MatchupRespons> matchupResponses, List<MatchupComment> matchupComments)
        {
            int result = -1;
            var newMatchupid = Guid.NewGuid();
            using (TransactionScope scope = new TransactionScope())
            {
                //on matchup en changeant l'ID

                matchup.CreationDate = DateTime.Now;
                //on cherche le matchupResponses et on le duplique aussi en changeant l'ID
                var oldMatchupResponsId = matchup.MatchupResponseId;
                var newMatchupResponsId = Guid.NewGuid();
                if (matchupResponses?.Count() > 0)
                {
                    foreach (var item in matchupResponses)
                    {
                        db.MatchupResponses.Add(new MatchupRespons()
                        {
                            ChampionId = item.ChampionId,
                            CreationDate = DateTime.Now,
                            MatchupResponseId = newMatchupResponsId
                        });
                    }
                }
                //on duplique les commentaires 
                if (matchupComments?.Count() > 0)
                {
                    foreach (var item in matchupComments)
                    {
                        db.MatchupComments.Add(new MatchupComment()
                        {
                            ChampionId = item.ChampionId,
                            CreationDate = DateTime.Now,
                            MatchupCommentId = Guid.NewGuid(),
                            CommentText = item.CommentText,
                            MatchupId = newMatchupid
                        });
                    }
                }

                var newMatchup = matchup.DuplicateWithNewIds(matchup, newMatchupid, newMatchupResponsId);
                bool isExist = new DALCalculation().isPerfectMatchupExist(newMatchup);
                if (!isExist)
                {
                    db.Matchups.Add(newMatchup);
                    result = db.SaveChanges();
                }
                scope.Complete();
            }

            return result;
        }

    }
}