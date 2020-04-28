using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.ApiServices
{
    public class PerformanceServices
    {
        public bool DidPlayerWin(MatchInfos matchInfos,Player player)
        {
            int teamId = GetPlayerTeam(matchInfos, player);
            string hasWin = matchInfos.teams.Where(x => x.teamId == teamId).Select(x => x.win).FirstOrDefault();
            if (hasWin.ToLower() == "win")
                return true;
            return false;
        }

        public int GetParticipantId(MatchInfos matchInfos,Player player)
        {
            int participantId = matchInfos.participantIdentities.Where(x => x.player.accountId == player.AccountId).Select(x => x.participantId).FirstOrDefault();
            return participantId;
        }

        public int GetPlayerTeam(MatchInfos matchInfos,Player player)
        {
            int participantId = GetParticipantId(matchInfos, player);
            int teamId = matchInfos.participants.Where(x => x.participantId == participantId).Select(x => x.teamId).FirstOrDefault();
            return teamId;
        }

        public int GetOpponentChampionId(MatchInfos matchInfos,Player player)
        {
            int teamId = GetPlayerTeam(matchInfos, player);
            int opponentTeamId = teamId == 100 ? 200 : 100;
            string playerRole = GlobalVar.getRoleById(player.Role);
            if (playerRole == "MID")
            {
                playerRole = "MIDDLE";
            }
            int opponentChampionId = matchInfos.participants.Where(x => x.timeline.lane == playerRole && x.teamId== opponentTeamId).Select(x => x.championId).FirstOrDefault();
            return opponentChampionId;
        }

        public List<int> GetListOpponentChampionId(List<MatchInfos> matches,Player player)
        {
            List<int> lchampionId = new List<int>();
            foreach (var matchInfo in matches)
            {
                int opponentChampionId = GetOpponentChampionId(matchInfo, player);
                if (opponentChampionId!= 0 && !lchampionId.Contains(opponentChampionId))
                {
                    lchampionId.Add(opponentChampionId);
                }
            }

            return lchampionId;
        }

        public List<MatchInfos> GetListMatchInfosByOpponentChampionId(List<MatchInfos> matches, Player player,int opponentChampionId)
        {
            List<MatchInfos> lmatchInfos = new List<MatchInfos>();
            foreach (var matchInfo in matches)
            {
                if (matchInfo.participants.Where(x => x.championId ==opponentChampionId).Any())
                {
                    lmatchInfos.Add(matchInfo);
                }
            }
            return lmatchInfos;
        }

    }
}