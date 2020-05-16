using FirstAPI.ApiServices;
using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.Models
{
    public class PerformancesViewModelBuilder
    {

        public TimelineViewModel BuildTimelineViewModel(MatchInfos matchInfos, Player player)
        {
            TimelineViewModel tvm = new TimelineViewModel();
            PerformanceServices ps = new PerformanceServices();
            TimelineServices ts = new TimelineServices();
            SoloQServices sq = new SoloQServices();
            int participantId = ps.GetParticipantId(matchInfos, player);
            var participant = ps.GetParticipantById(matchInfos, participantId);

            var opponent = ps.GetOpponentNameByOpponentId(matchInfos, player);
            var oppponentParticipantId = opponent.Item2;
            var opponantParticipant = ps.GetParticipantById(matchInfos, oppponentParticipantId);
            var frames = sq.GetTimeLinesMatchInfos(matchInfos.gameId.ToString());

            var timeline = matchInfos.participants.Where(x => x.participantId == participantId).FirstOrDefault().timeline;
            tvm.participantId = timeline.participantId;

            tvm.kills = matchInfos.participants.Where(x => x.participantId == participantId).FirstOrDefault().stats.kills;
            tvm.deaths = matchInfos.participants.Where(x => x.participantId == participantId).FirstOrDefault().stats.deaths;
            tvm.assists = matchInfos.participants.Where(x => x.participantId == participantId).FirstOrDefault().stats.assists;
            tvm.opponentName = opponent.Item1;
            tvm.timestamp = matchInfos.gameCreation;
            tvm.gameId = matchInfos.gameId;

            var spell = ps.GetSummonerSpellsByParticipantId(matchInfos, participantId);
            var opponentSpell = ps.GetSummonerSpellsByParticipantId(matchInfos, oppponentParticipantId);
            tvm.spell1Id = spell.Item1;
            tvm.spell2Id = spell.Item2;
            tvm.opponentSpell1Id = opponentSpell.Item1;
            tvm.opponentSpell2Id = opponentSpell.Item2;

            tvm.primaryKey = ps.GetPrimaryRune(matchInfos, participantId);
            tvm.primaryKeyStyle = ps.GetPrimaryStyleRune(matchInfos, participantId);
            tvm.opponentPrimaryKey = ps.GetPrimaryRune(matchInfos, oppponentParticipantId);
            tvm.opponentPrimaryKeyStyle = ps.GetPrimaryStyleRune(matchInfos, oppponentParticipantId);

            
            tvm.creepsPerMinDeltas = new CreepsPerMinDeltasViewModel();
            tvm.creepsPerMinDeltas.firstPartTime = timeline.creepsPerMinDeltas?.firstPartTime;
            tvm.creepsPerMinDeltas.secondPartTime = timeline.creepsPerMinDeltas?.secondPartTime;

            tvm.csDiffPerMinDeltas = new CsDiffPerMinDeltasViewModel();

            //results are not working well or bad calculated
            //tvm.csDiffPerMinDeltas.firstPartTime = timeline.csDiffPerMinDeltas?.firstPartTime;
            //tvm.csDiffPerMinDeltas.secondPartTime = timeline.csDiffPerMinDeltas?.secondPartTime;

            tvm.csDiffPerMinDeltas.fiveMin = ts.getCsDiffByTimingMark(frames, participantId, oppponentParticipantId, 5);
            tvm.csDiffPerMinDeltas.tenMin = ts.getCsDiffByTimingMark(frames, participantId, oppponentParticipantId, 10);
            tvm.csDiffPerMinDeltas.fifteenMin = ts.getCsDiffByTimingMark(frames, participantId, oppponentParticipantId, 15);
            tvm.csDiffPerMinDeltas.twentyMin = ts.getCsDiffByTimingMark(frames, participantId, oppponentParticipantId, 20);

            tvm.goldPerMinDeltas = new GoldPerMinDeltasViewModel();
            tvm.goldPerMinDeltas.firstPartTime = timeline.goldPerMinDeltas?.firstPartTime;
            tvm.goldPerMinDeltas.secondPartTime = timeline.goldPerMinDeltas?.secondPartTime;

            tvm.damageTakenDiffPerMinDeltas = new DamageTakenDiffPerMinDeltasViewModel();
            tvm.damageTakenDiffPerMinDeltas.firstPartTime = timeline.damageTakenDiffPerMinDeltas?.firstPartTime;
            tvm.damageTakenDiffPerMinDeltas.secondPartTime = timeline.damageTakenDiffPerMinDeltas?.secondPartTime;

            tvm.damageTakenPerMinDeltas = new DamageTakenPerMinDeltasViewModel();
            tvm.damageTakenPerMinDeltas.firstPartTime = timeline.damageTakenPerMinDeltas?.firstPartTime;
            tvm.damageTakenPerMinDeltas.secondPartTime = timeline.damageTakenPerMinDeltas?.secondPartTime;

            tvm.xpDiffPerMinDeltas = new XpDiffPerMinDeltasViewModel();
            //tvm.xpDiffPerMinDeltas.firstPartTime = timeline.xpDiffPerMinDeltas?.firstPartTime;
            //tvm.xpDiffPerMinDeltas.secondPartTime = timeline.xpDiffPerMinDeltas?.secondPartTime;

            tvm.xpDiffPerMinDeltas.fiveMin = ts.getXpDiffByTimingMark(frames, participantId, oppponentParticipantId, 5);
            tvm.xpDiffPerMinDeltas.tenMin = ts.getXpDiffByTimingMark(frames, participantId, oppponentParticipantId, 10);
            tvm.xpDiffPerMinDeltas.fifteenMin = ts.getXpDiffByTimingMark(frames, participantId, oppponentParticipantId, 15);
            tvm.xpDiffPerMinDeltas.twentyMin = ts.getXpDiffByTimingMark(frames, participantId, oppponentParticipantId, 20);

            tvm.XpPerMinDeltas = new XpPerMinDeltasViewModel();
            tvm.XpPerMinDeltas.firstPartTime = timeline.XpPerMinDeltas?.firstPartTime;
            tvm.XpPerMinDeltas.secondPartTime = timeline.XpPerMinDeltas?.secondPartTime;

            tvm.deathCount = new DeathCountViewModel();
            tvm.deathCount.fiveMin = ts.GetNbDeathBetweenTimingMark(frames, participantId, 0, 5);
            tvm.deathCount.tenMin = ts.GetNbDeathBetweenTimingMark(frames, participantId, 5, 10);
            tvm.deathCount.fifteenMin = ts.GetNbDeathBetweenTimingMark(frames, participantId, 10, 15);
            tvm.deathCount.twentyMin = ts.GetNbDeathBetweenTimingMark(frames, participantId, 15, 20);
            tvm.deathCount.twentyFiveMin = ts.GetNbDeathBetweenTimingMark(frames, participantId, 20, 0);
            //tvm.deathCount.thirtyMin = ts.GetNbDeathBetweenTimingMark(frames, participantId, 25, 30);

            if (player.Role == 5)
            {
                tvm.wardDestroyedCount = new WardDestroyedCountViewModel();

                tvm.wardDestroyedCount.tenMin = ts.GetNbWardDestroyedBetweenTimingMark(frames, participantId, 0, 10);
                tvm.wardDestroyedCount.fifteenMin = ts.GetNbWardDestroyedBetweenTimingMark(frames, participantId, 10, 15);
                tvm.wardDestroyedCount.twentyMin = ts.GetNbWardDestroyedBetweenTimingMark(frames, participantId, 15, 20);
                tvm.wardDestroyedCount.twentyTwoMin = ts.GetNbWardDestroyedBetweenTimingMark(frames, participantId, 20, 22);
                tvm.wardDestroyedCount.twentyFourMin = ts.GetNbWardDestroyedBetweenTimingMark(frames, participantId, 22, 24);
                tvm.wardDestroyedCount.twentySixMin = ts.GetNbWardDestroyedBetweenTimingMark(frames, participantId, 24, 26);
                tvm.wardDestroyedCount.twentyEightMin = ts.GetNbWardDestroyedBetweenTimingMark(frames, participantId, 26, 28);
                tvm.wardDestroyedCount.thirtyrMin = ts.GetNbWardDestroyedBetweenTimingMark(frames, participantId, 28, 30);

                tvm.wardPutCount = new WardPutCountViewModel();

                tvm.wardPutCount.tenMin = ts.GetNbWardPutBetweenTimingMark(frames, participantId, 0, 10);
                tvm.wardPutCount.fifteenMin = ts.GetNbWardPutBetweenTimingMark(frames, participantId, 10, 15);
                tvm.wardPutCount.twentyMin = ts.GetNbWardPutBetweenTimingMark(frames, participantId, 15, 20);
                tvm.wardPutCount.twentyTwoMin = ts.GetNbWardPutBetweenTimingMark(frames, participantId, 20, 22);
                tvm.wardPutCount.twentyFourMin = ts.GetNbWardPutBetweenTimingMark(frames, participantId, 22, 24);
                tvm.wardPutCount.twentySixMin = ts.GetNbWardPutBetweenTimingMark(frames, participantId, 24, 26);
                tvm.wardPutCount.twentyEightMin = ts.GetNbWardPutBetweenTimingMark(frames, participantId, 26, 28);
                tvm.wardPutCount.thirtyrMin = ts.GetNbWardPutBetweenTimingMark(frames, participantId, 28, 30);

                tvm.pinkPutCount = new PinkPutCountViewModel();
                tvm.pinkPutCount.tenMin = ts.GetNbPinkPutBetweenTimingMark(frames, participantId, 0, 10);
                tvm.pinkPutCount.fifteenMin = ts.GetNbPinkPutBetweenTimingMark(frames, participantId, 10, 15);
                tvm.pinkPutCount.twentyMin = ts.GetNbPinkPutBetweenTimingMark(frames, participantId, 15, 20);
                tvm.pinkPutCount.twentyTwoMin = ts.GetNbPinkPutBetweenTimingMark(frames, participantId, 20, 22);
                tvm.pinkPutCount.twentyFourMin = ts.GetNbPinkPutBetweenTimingMark(frames, participantId, 22, 24);
                tvm.pinkPutCount.twentySixMin = ts.GetNbPinkPutBetweenTimingMark(frames, participantId, 24, 26);
                tvm.pinkPutCount.twentyEightMin = ts.GetNbPinkPutBetweenTimingMark(frames, participantId, 26, 28);
                tvm.pinkPutCount.thirtyrMin = ts.GetNbPinkPutBetweenTimingMark(frames, participantId, 28, 30);

            }

            tvm.lane = timeline.lane;
            tvm.role = timeline.role;
            tvm.participantId = timeline.participantId;


            return tvm;

        }

        public List<PerformancesViewModel> BuildPerformanceViewModel(List<Match> matches, Player player)
        {
            List<PerformancesViewModel> lpvm = new List<PerformancesViewModel>();
            SoloQServices sq = new SoloQServices();
            PerformanceServices ps = new PerformanceServices();

            List<Match> games = ps.GetListMatchByRole(matches, player).OrderByDescending(x => x.gameId).ToList();
            List<int> listChampions = games.Select(x => x.championId).Distinct().ToList();
            foreach (var championId in listChampions)
            {
                List<Match> listMatch = games.Where(x => x.championId == championId).ToList();
                List<MatchInfos> listMatchInfos = sq.GetListMatchInfos(listMatch);

                var lMatchupHistory = ps.GetListOpponentChampionIdWtihListMatch(listMatchInfos, player);
                foreach (var matchupHistory in lMatchupHistory)
                {
                    PerformancesViewModel pvm = new PerformancesViewModel();
                    pvm.championId = championId;
                    pvm.timelines = new List<TimelineViewModel>();
                    pvm.opponentChampionId = matchupHistory.opponentChampionId;
                    foreach (var matchInfos in matchupHistory.listMatch)
                    {
                        var timeline = BuildTimelineViewModel(matchInfos, player);

                        if (ps.DidPlayerWin(matchInfos, player))
                        {
                            pvm.nbVictory++;
                            timeline.win = true;
                        }
                        else
                        {
                            pvm.nbDefeat++;
                            timeline.win = false;
                        }

                        pvm.timelines.Add(timeline);
                    }
                    lpvm.Add(pvm);
                }
            }
            return lpvm;
        }
    }
}