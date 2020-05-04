using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.ApiServices
{
    public class PerformanceServices
    {
        public bool DidPlayerWin(MatchInfos matchInfos, Player player)
        {
            int teamId = GetPlayerTeam(matchInfos, player);
            string hasWin = matchInfos.teams.Where(x => x.teamId == teamId).Select(x => x.win).FirstOrDefault();
            if (hasWin.ToLower() == "win")
                return true;
            return false;
        }

        public int GetParticipantId(MatchInfos matchInfos, Player player)
        {
            int participantId = matchInfos.participantIdentities.Where(x => x.player.accountId == player.AccountId).Select(x => x.participantId).FirstOrDefault();
            return participantId;
        }

        public int GetPlayerTeam(MatchInfos matchInfos, Player player)
        {
            int participantId = GetParticipantId(matchInfos, player);
            int teamId = matchInfos.participants.Where(x => x.participantId == participantId).Select(x => x.teamId).FirstOrDefault();
            return teamId;
        }

        public Tuple<string,int> GetOpponentNameByOpponentId(MatchInfos matchInfos, Player player)
        {
            int teamId = GetPlayerTeam(matchInfos, player);
            int opponentTeamId = teamId == 100 ? 200 : 100;
            string playerRole = GlobalVar.getRoleById(player.Role);
            int opponentParticipantId = 0;
            if (playerRole == "MID")
            {
                playerRole = "MIDDLE";
                opponentParticipantId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.teamId == opponentTeamId).Select(x => x.participantId).FirstOrDefault();
            }
            else if (playerRole == "SUPPORT")
            {
                playerRole = "BOTTOM";
                string playerLane = "DUO_SUPPORT";
                opponentParticipantId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.timeline.role == playerLane && x.teamId == opponentTeamId).Select(x => x.participantId).FirstOrDefault();
            }
            else if (playerRole == "BOTTOM")
            {
                string playerLane = "DUO_CARRY";
                opponentParticipantId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.timeline.role == playerLane && x.teamId == opponentTeamId).Select(x => x.participantId).FirstOrDefault();
                if (opponentParticipantId == 0) // if thoses lines code below , change it also in method "GetOpponentChampionId"
                {
                    playerLane = "SOLO";
                    playerRole = "BOTTOM";
                    opponentParticipantId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.timeline.role == playerLane && x.teamId == opponentTeamId).Select(x => x.participantId).FirstOrDefault();
                }
            }
            else
            {
                opponentParticipantId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.teamId == opponentTeamId).Select(x => x.participantId).FirstOrDefault();
            }

            return new Tuple<string, int>(matchInfos.participantIdentities.Where(x => x.participantId == opponentParticipantId).Select(x => x.player.summonerName).FirstOrDefault(),opponentParticipantId);
        }

        public int GetOpponentChampionId(MatchInfos matchInfos, Player player)
        {
            int teamId = GetPlayerTeam(matchInfos, player);
            int opponentTeamId = teamId == 100 ? 200 : 100;
            string playerRole = GlobalVar.getRoleById(player.Role);
            int opponentChampionId = 0;
            if (playerRole == "MID")
            {
                playerRole = "MIDDLE";
                opponentChampionId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.teamId == opponentTeamId).Select(x => x.championId).FirstOrDefault();

            }
            else if (playerRole == "SUPPORT")
            {
                playerRole = "BOTTOM";
                string playerLane = "DUO_SUPPORT";
                opponentChampionId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.timeline.role == playerLane && x.teamId == opponentTeamId).Select(x => x.championId).FirstOrDefault();
            }
            else if (playerRole == "BOTTOM")
            {
                string playerLane = "DUO_CARRY";
                opponentChampionId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.timeline.role == playerLane && x.teamId == opponentTeamId).Select(x => x.championId).FirstOrDefault();
                if (opponentChampionId == 0)// if thoses lines code below , change it also in method "GetOpponentNameByOpponentId"
                {
                    playerLane = "SOLO";
                    playerRole = "BOTTOM";
                    opponentChampionId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.timeline.role == playerLane && x.teamId == opponentTeamId).Select(x => x.championId).FirstOrDefault();
                }
            }
            else
            {
                opponentChampionId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.teamId == opponentTeamId).Select(x => x.championId).FirstOrDefault();
            }
            return opponentChampionId;
        }

        public List<int> GetListOpponentChampionId(List<MatchInfos> matches, Player player)
        {
            List<int> lchampionId = new List<int>();
            foreach (var matchInfo in matches)
            {
                int opponentChampionId = GetOpponentChampionId(matchInfo, player);
                if (opponentChampionId != 0 && !lchampionId.Contains(opponentChampionId))
                {
                    lchampionId.Add(opponentChampionId);
                }
            }

            return lchampionId;
        }

        public List<MatchupHistory> GetListOpponentChampionIdWtihListMatch(List<MatchInfos> matches, Player player)
        {
            List<MatchupHistory> ltest = new List<MatchupHistory>();
            foreach (var matchInfos in matches)
            {
                MatchupHistory matchupHistory = new MatchupHistory();
                int opponentChampionId = GetOpponentChampionId(matchInfos, player);
                if (opponentChampionId != 0)
                {
                    matchupHistory = ltest.Where(x => x.opponentChampionId == opponentChampionId).FirstOrDefault();
                    if (matchupHistory == null)
                    {
                        matchupHistory = new MatchupHistory();
                        matchupHistory.opponentChampionId = opponentChampionId;
                        matchupHistory.listMatch = new List<MatchInfos>();
                    }
                    matchupHistory.listMatch.Add(matchInfos);
                }
                if (matchupHistory.opponentChampionId != 0 && !ltest.Where(x => x.opponentChampionId == opponentChampionId).Any())
                {
                    ltest.Add(matchupHistory);
                }
            }

            return ltest;
        }

        public List<MatchInfos> GetListMatchInfosByOpponentChampionId(List<MatchInfos> matches, Player player, int opponentChampionId)
        {
            List<MatchInfos> lmatchInfos = new List<MatchInfos>();
            foreach (var matchInfo in matches)
            {
                if (matchInfo.participants.Where(x => x.championId == opponentChampionId).Any())
                {
                    lmatchInfos.Add(matchInfo);

                }
            }

            return lmatchInfos;
        }

    }

    public class MatchupHistory
    {
        public int opponentChampionId { get; set; }
        public List<MatchInfos> listMatch { get; set; }
    }
}