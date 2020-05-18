using FirstAPI.ApiServices;
using FirstAPI.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.Models
{
    public class WinratesViewModelBuilder
    {
        public WinratesViewModel BuildWinratesViewModel(int nbGames, Player player)
        {
            SoloQServices sq = new SoloQServices();
            WinratesViewModel wrm=null;
            var ps = new PerformanceServices();
            var matches = sq.GetSoloQHistories(player.AccountId, nbGames).OrderByDescending(x => x.timestamp).ToList();

            if (matches != null)
            {
                if (player.Role != 0)
                {
                    matches = ps.GetListMatchByRole(matches, player);
                }
                wrm = new WinratesViewModel();
                wrm.nickname = player.Nickname;
                wrm.blueSide = new WinratesViewModel.Side();
                wrm.redSide = new WinratesViewModel.Side();
                wrm.totalGames = nbGames;
                wrm.role = GlobalVar.getRoleById(player.Role);

                foreach (var match in matches)
                {

                    var matchInfos = sq.GetMatchInfo(match.gameId.ToString());
                    //savoir dans quel side il était et si il a win
                    var team = ps.GetPlayerTeam(matchInfos, player);
                    var isWin = matchInfos.teams.Where(x => x.teamId == team).FirstOrDefault()?.win;
                    bool isBlueSideWin = false;
                    if (team == 100)
                    {
                        if (isWin.ToLower() == "win")
                        {
                            wrm.blueSide.totalGames++;
                            wrm.blueSide.nbWin++;
                            wrm.totalGamesOnlyMainRole++;
                            isBlueSideWin = true;
                        }
                        else if (isWin.ToLower() == "fail")
                        {
                            wrm.blueSide.totalGames++;
                            wrm.blueSide.nbLoss++;
                            wrm.totalGamesOnlyMainRole++;
                        }

                    }
                    else if (team == 200)
                    {
                        if (isWin.ToLower() == "win")
                        {
                            wrm.redSide.totalGames++;
                            wrm.redSide.nbWin++;
                            wrm.totalGamesOnlyMainRole++;
                        }
                        else if (isWin.ToLower() == "fail")
                        {
                            wrm.redSide.totalGames++;
                            wrm.redSide.nbLoss++;
                            wrm.totalGamesOnlyMainRole++;
                            isBlueSideWin = true;
                        }
                    }
                    int participantId = ps.GetParticipantId(matchInfos, player);
                    if (participantId != 0)
                    {
                        CalculateDraftPosition(wrm, matchInfos, participantId, isBlueSideWin);
                    }
                }
            }

            return wrm;
        }

        public void CalculateDraftPosition(WinratesViewModel wrm, MatchInfos match, int participantId, bool isBlueSideWin)
        {

            switch (participantId)
            {
                case 1:
                    wrm.blueSide.nbPick1++;
                    if (isBlueSideWin)
                    {
                        wrm.blueSide.nbWinPick1++;
                    }
                    break;
                case 2:
                    wrm.blueSide.nbPick2++;
                    if (isBlueSideWin)
                    {
                        wrm.blueSide.nbWinPick2++;
                    }
                    break;
                case 3:
                    wrm.blueSide.nbPick3++;
                    if (isBlueSideWin)
                    {
                        wrm.blueSide.nbWinPick3++;
                    }
                    break;
                case 4:
                    wrm.blueSide.nbPick4++;
                    if (isBlueSideWin)
                    {
                        wrm.blueSide.nbWinPick4++;
                    }
                    break;
                case 5:
                    wrm.blueSide.nbPick5++;
                    if (isBlueSideWin)
                    {
                        wrm.blueSide.nbWinPick5++;
                    }
                    break;
                case 6:
                    wrm.redSide.nbPick6++;
                    if (!isBlueSideWin)
                    {
                        wrm.redSide.nbWinPick6++;
                    }
                    break;
                case 7:
                    wrm.redSide.nbPick7++;
                    if (!isBlueSideWin)
                    {
                        wrm.redSide.nbWinPick7++;
                    }
                    break;
                case 8:
                    wrm.redSide.nbPick8++;
                    if (!isBlueSideWin)
                    {
                        wrm.redSide.nbWinPick8++;
                    }
                    break;
                case 9:
                    wrm.redSide.nbPick9++;
                    if (!isBlueSideWin)
                    {
                        wrm.redSide.nbWinPick9++;
                    }
                    break;
                case 10:
                    wrm.redSide.nbPick10++;
                    if (!isBlueSideWin)
                    {
                        wrm.redSide.nbWinPick10++;
                    }
                    break;
                default:
                    break;
            }

        }
    }
}