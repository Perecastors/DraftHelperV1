using FirstAPI.ApiServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace FirstAPI.Models
{
    public class SoloQViewModelBuilder
    {

        public List<MatchViewModel> BuildSoloQHistories(List<Match> matches)
        {
            List<MatchViewModel> lmvm = new List<MatchViewModel>();
            SoloQServices sqs = new SoloQServices();
            foreach (var match in matches)
            {
                MatchViewModel mvm = new MatchViewModel();
                mvm.championId = match.championId;
                mvm.lane = match.lane;
                mvm.timestamp = match.timestamp;
                mvm.role = match.role;
                mvm.lane = match.lane;

                mvm.matchInfo = new MatchInfosViewModel();
                MatchInfos matchInfo = sqs.GetMatchInfo(match.gameId.ToString());
                mvm.matchInfo.gameCreation = matchInfo.gameCreation;
                mvm.matchInfo.gameDuration = matchInfo.gameDuration;

                Participant participant = matchInfo.participants.Where(x => x.championId == match.championId).FirstOrDefault();
                mvm.participant = BuildParticipantViewModel(participant);
                mvm.participant.stats = BuildStatsViewModel(participant.stats);
                lmvm.Add(mvm);
            }

            return lmvm;
        }

        public MatchInfosViewModel BuildMatchInfosViewModel(MatchInfos matchInfos)
        {
            MatchInfosViewModel mivm = new MatchInfosViewModel();
            mivm.gameCreation = matchInfos.gameCreation;
            mivm.gameDuration = matchInfos.gameDuration;
            mivm.gameId = matchInfos.gameId;
            mivm.gameVersion = matchInfos.gameVersion;
            mivm.teams = BuildListTeamViewModel(matchInfos.teams);

            return mivm;

        }

        public List<TeamViewModel> BuildListTeamViewModel(List<Team> teams)
        {
            List<TeamViewModel> ltvm = new List<TeamViewModel>();
            foreach (var team in teams)
            {
                ltvm.Add(BuildTeamViewModel(team));
            }
            return ltvm;
        }

        public TeamViewModel BuildTeamViewModel(Team team)
        {
            TeamViewModel tvm = new TeamViewModel();
            tvm.baronKills = team.baronKills;
            tvm.dragonKills = team.dragonKills;
            tvm.firstBaron = team.firstBaron;
            tvm.firstBlood = team.firstBlood;
            tvm.firstInhibitor = team.firstInhibitor;
            tvm.firstRiftHerald = team.firstRiftHerald;
            tvm.firstTower = team.firstTower;
            tvm.inhibitorKills = team.inhibitorKills;
            tvm.riftHeraldKills = team.riftHeraldKills;
            tvm.teamId = tvm.teamId;
            tvm.towerKills = tvm.towerKills;
            tvm.win = team.win;
            tvm.bans = BuildListBanViewModel(team.bans);

            return tvm;
        }

        public List<BanViewModel> BuildListBanViewModel(List<Ban> bans)
        {
            List<BanViewModel> lbvm= new List<BanViewModel>();
            foreach (var ban in bans)
            {
                lbvm.Add(BuildBanViewModel(ban));
            }
            return lbvm;
        }

        public BanViewModel BuildBanViewModel(Ban ban)
        {
            BanViewModel bvm = new BanViewModel();
            bvm.championId = ban.championId;
            bvm.pickTurn = ban.pickTurn;
            return bvm;
        }

        public ParticipantViewModel BuildParticipantViewModel(Participant participant)
        {
            ParticipantViewModel pvm = new ParticipantViewModel();

            pvm.championId = participant.championId;
            pvm.participantId = participant.participantId;
            pvm.spell1Id = participant.spell1Id;
            pvm.spell2Id = participant.spell2Id;
            pvm.teamId = participant.teamId;
            return pvm;
        }


        public StatsViewModel BuildStatsViewModel(Stats stats)
        {
            StatsViewModel statsVm = new StatsViewModel();
            statsVm.assists = stats.assists;
            statsVm.champLevel = stats.champLevel;
            statsVm.deaths = stats.deaths;
            statsVm.goldEarned = stats.goldEarned;
            statsVm.goldSpent = stats.goldSpent;
            statsVm.item0 = stats.item0;
            statsVm.item1 = stats.item1;
            statsVm.item2 = stats.item2;
            statsVm.item3 = stats.item3;
            statsVm.item4 = stats.item4;
            statsVm.item5 = stats.item5;
            statsVm.item6 = stats.item6;
            statsVm.kills = stats.kills;
            statsVm.longestTimeSpentLiving = stats.longestTimeSpentLiving;
            statsVm.magicalDamageTaken = stats.magicalDamageTaken;
            statsVm.magicDamageDealt = stats.magicDamageDealt;
            statsVm.magicDamageDealtToChampions = stats.magicDamageDealtToChampions;
            statsVm.neutralMinionsKilled = stats.neutralMinionsKilled;
            statsVm.neutralMinionsKilledEnemyJungle = stats.neutralMinionsKilledEnemyJungle;
            statsVm.neutralMinionsKilledTeamJungle = stats.neutralMinionsKilledTeamJungle;
            statsVm.participantId = stats.participantId;
            statsVm.perk0 = stats.perk0;
            statsVm.perk0Var1 = stats.perk0Var1;
            statsVm.perk0Var2 = stats.perk0Var2;
            statsVm.perk0Var3 = stats.perk0Var3;
            statsVm.perk1 = stats.perk1;
            statsVm.perk1Var1 = stats.perk1Var1;
            statsVm.perk1Var2 = stats.perk1Var2;
            statsVm.perk1Var3 = stats.perk1Var3;
            statsVm.perk2 = stats.perk2;
            statsVm.perk2Var1 = stats.perk2Var1;
            statsVm.perk2Var2 = stats.perk2Var2;
            statsVm.perk2Var3 = stats.perk2Var3;
            statsVm.perk3 = stats.perk3;
            statsVm.perk3Var1 = stats.perk3Var1;
            statsVm.perk3Var2 = stats.perk3Var2;
            statsVm.perk3Var3 = stats.perk3Var3;
            statsVm.perk4 = stats.perk4;
            statsVm.perk4Var1 = stats.perk4Var1;
            statsVm.perk4Var2 = stats.perk4Var2;
            statsVm.perk4Var3 = stats.perk4Var3;
            statsVm.perk5 = stats.perk5;
            statsVm.perk5Var1 = stats.perk5Var1;
            statsVm.perk5Var2 = stats.perk5Var2;
            statsVm.perk5Var3 = stats.perk5Var3;
            statsVm.perkPrimaryStyle = stats.perkPrimaryStyle;
            statsVm.perkSubStyle = stats.perkSubStyle;
            statsVm.physicalDamageDealt = stats.physicalDamageDealt;
            statsVm.physicalDamageDealtToChampions = stats.physicalDamageDealtToChampions;
            statsVm.physicalDamageTaken = stats.physicalDamageTaken;
            statsVm.sightWardsBouthInGame = stats.sightWardsBouthInGame;
            statsVm.statPerk0 = stats.statPerk0;
            statsVm.statPerk1 = stats.statPerk1;
            statsVm.statPerk2 = stats.statPerk2;
            statsVm.timeCCingOthers = stats.timeCCingOthers;
            statsVm.totalDamageDealt = statsVm.totalDamageDealt;
            statsVm.totalDamageDealtToChampions = statsVm.totalDamageDealtToChampions;
            statsVm.totalDamageTaken = stats.totalDamageTaken;
            statsVm.totalMinionsKilled = stats.totalMinionsKilled;
            statsVm.totalTimeCrowdControlDealt = stats.trueDamageDealt;
            statsVm.trueDamageDealt = statsVm.trueDamageDealt;
            statsVm.trueDamageDealtToChampions = stats.trueDamageDealtToChampions;
            statsVm.trueDamageTaken = stats.trueDamageTaken;
            statsVm.visionScore = stats.visionScore;
            statsVm.visionWardsBoughtInGame = statsVm.visionWardsBoughtInGame;
            statsVm.wardsKilled = stats.wardsKilled;
            statsVm.wardsPlaced = stats.wardsPlaced;
            statsVm.win = stats.win;

            return statsVm;
        }


    }
}