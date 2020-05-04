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

        private List<Match> GetListMatchByRole(List<Match> matches, Player player)
        {
            List<Match> games = new List<Match>();
            string playerRole = GlobalVar.getRoleById(player.Role);
            if (playerRole == "SUPPORT")
            {
                games = matches.Where(x => x.lane.ToUpper() == "BOTTOM").ToList();
            }
            else
            {
                games = matches.Where(x => x.lane.ToUpper() == playerRole).ToList();
            }

            return games;
        }



        public TimelineViewModel BuildTimelineViewModel(MatchInfos matchInfos, Player player)
        {
            TimelineViewModel tvm = new TimelineViewModel();
            PerformanceServices ps = new PerformanceServices();
            TimelineServices ts = new TimelineServices();
            SoloQServices sq = new SoloQServices();
            int participantId = ps.GetParticipantId(matchInfos, player);
            var opponent = ps.GetOpponentNameByOpponentId(matchInfos, player);
            var oppponentParticipantId = opponent.Item2;
            var frames = sq.GetTimeLinesMatchInfos(matchInfos.gameId.ToString());

            var timeline = matchInfos.participants.Where(x => x.participantId == participantId).FirstOrDefault().timeline;
            tvm.participantId = timeline.participantId;

            tvm.kills = matchInfos.participants.Where(x => x.participantId == participantId).FirstOrDefault().stats.kills;
            tvm.deaths = matchInfos.participants.Where(x => x.participantId == participantId).FirstOrDefault().stats.deaths;
            tvm.assists = matchInfos.participants.Where(x => x.participantId == participantId).FirstOrDefault().stats.assists;
            tvm.opponentName = opponent.Item1;
            tvm.timestamp = matchInfos.gameCreation;
            tvm.gameId = matchInfos.gameId;

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


            tvm.XpPerMinDeltas = new XpPerMinDeltasViewModel();
            tvm.XpPerMinDeltas.firstPartTime = timeline.XpPerMinDeltas?.firstPartTime;
            tvm.XpPerMinDeltas.secondPartTime = timeline.XpPerMinDeltas?.secondPartTime;

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

            List<Match> games = GetListMatchByRole(matches, player).OrderByDescending(x => x.gameId).ToList();
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