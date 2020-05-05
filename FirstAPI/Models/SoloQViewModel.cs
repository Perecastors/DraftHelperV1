using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.Models
{
    public class SoloQViewModel
    {
        public List<MatchViewModel> listMatch;
    }

    public class MatchViewModel
    {
        public string platformId { get; set; }
        public long gameId { get; set; }
        public int championId { get; set; }
        public int queue { get; set; }
        public int season { get; set; }
        public long timestamp { get; set; }
        public string role { get; set; }
        public string lane { get; set; }
        public MatchInfosViewModel matchInfo { get; set; }
        public ParticipantViewModel participant { get; set; }
    }

    public class MatchInfosViewModel
    {
        public long gameId { get; set; }
        public long gameCreation { get; set; }
        public int gameDuration { get; set; }
        public string gameVersion { get; set; }
        public List<TeamViewModel> teams { get; set; }
        public List<ParticipantViewModel> participants { get; set; }
    }

    public class TeamViewModel
    {
        public int teamId { get; set; }
        public string win { get; set; }
        public bool firstBlood { get; set; }
        public bool firstTower { get; set; }
        public bool firstInhibitor { get; set; }
        public bool firstBaron { get; set; }
        public bool firstRiftHerald { get; set; }
        public int towerKills { get; set; }
        public int inhibitorKills { get; set; }
        public int baronKills { get; set; }
        public int dragonKills { get; set; }
        public int riftHeraldKills { get; set; }
        public List<BanViewModel> bans { get; set; }
    }

    public class BanViewModel
    {
        public int championId { get; set; }
        public int pickTurn { get; set; }
    }

    public class ParticipantViewModel
    {
        public int participantId { get; set; }
        public int teamId { get; set; }
        public int championId { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public StatsViewModel stats { get; set; }
    }

    public class StatsViewModel
    {
        public int participantId { get; set; }
        public bool win { get; set; }
        public int item0 { get; set; }
        public int item1 { get; set; }
        public int item2 { get; set; }
        public int item3 { get; set; }
        public int item4 { get; set; }
        public int item5 { get; set; }
        public int item6 { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }
        public int longestTimeSpentLiving { get; set; }
        public int totalDamageDealt { get; set; }
        public int magicDamageDealt { get; set; }
        public int physicalDamageDealt { get; set; }
        public int trueDamageDealt { get; set; }
        public int totalDamageDealtToChampions { get; set; }
        public int magicDamageDealtToChampions { get; set; }
        public int physicalDamageDealtToChampions { get; set; }
        public int trueDamageDealtToChampions { get; set; }
        public int visionScore { get; set; }
        public int timeCCingOthers { get; set; }
        public int totalDamageTaken { get; set; }
        public int magicalDamageTaken { get; set; }
        public int physicalDamageTaken { get; set; }
        public int trueDamageTaken { get; set; }
        public int goldEarned { get; set; }
        public int goldSpent { get; set; }
        public int totalMinionsKilled { get; set; }
        public int neutralMinionsKilled { get; set; }
        public int neutralMinionsKilledTeamJungle { get; set; }
        public int neutralMinionsKilledEnemyJungle { get; set; }
        public int totalTimeCrowdControlDealt { get; set; }
        public int champLevel { get; set; }
        public int visionWardsBoughtInGame { get; set; }
        public int sightWardsBouthInGame { get; set; }
        public int wardsPlaced { get; set; }
        public int wardsKilled { get; set; }
        public int perk0 { get; set; }
        public int perk0Var1 { get; set; }
        public int perk0Var2 { get; set; }
        public int perk0Var3 { get; set; }
        public int perk1 { get; set; }
        public int perk1Var1 { get; set; }
        public int perk1Var2 { get; set; }
        public int perk1Var3 { get; set; }
        public int perk2 { get; set; }
        public int perk2Var1 { get; set; }
        public int perk2Var2 { get; set; }
        public int perk2Var3 { get; set; }
        public int perk3 { get; set; }
        public int perk3Var1 { get; set; }
        public int perk3Var2 { get; set; }
        public int perk3Var3 { get; set; }
        public int perk4 { get; set; }
        public int perk4Var1 { get; set; }
        public int perk4Var2 { get; set; }
        public int perk4Var3 { get; set; }
        public int perk5 { get; set; }
        public int perk5Var1 { get; set; }
        public int perk5Var2 { get; set; }
        public int perk5Var3 { get; set; }
        public int perkPrimaryStyle { get; set; }
        public int perkSubStyle { get; set; }
        public int statPerk0 { get; set; }
        public int statPerk1 { get; set; }
        public int statPerk2 { get; set; }
    }

    public class TimelineViewModel
    {
        public int participantId { get; set; }
        public string role { get; set; }
        public string lane { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }
        public string opponentName { get; set; }
        public long timestamp { get; set; }
        public bool win { get; set; }
        public long gameId { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public int opponentSpell1Id { get; set; }
        public int opponentSpell2Id { get; set; }
        public CreepsPerMinDeltasViewModel creepsPerMinDeltas { get; set; }
        public XpDiffPerMinDeltasViewModel xpDiffPerMinDeltas { get; set; }
        public GoldPerMinDeltasViewModel goldPerMinDeltas { get; set; }
        public CsDiffPerMinDeltasViewModel csDiffPerMinDeltas { get; set; }
        public DamageTakenDiffPerMinDeltasViewModel damageTakenDiffPerMinDeltas { get; set; }
        public DamageTakenPerMinDeltasViewModel damageTakenPerMinDeltas { get; set; }
        public XpPerMinDeltasViewModel XpPerMinDeltas { get; set; }
        public DeathCountViewModel deathCount { get; set; }

    }

    public class CreepsPerMinDeltasViewModel
    {
        public double? firstPartTime { get; set; }
        public double? secondPartTime { get; set; }  
    }

    public class XpPerMinDeltasViewModel
    {
        public double? firstPartTime { get; set; }
        public double? secondPartTime { get; set; }

    }

    public class GoldPerMinDeltasViewModel
    {
        public double? firstPartTime { get; set; }
        public double? secondPartTime { get; set; }
    }

    public class CsDiffPerMinDeltasViewModel
    {
        public double? firstPartTime { get; set; }
        public double? secondPartTime { get; set; }

        public double? fiveMin { get; set; }
        public double? tenMin { get; set; }
        public double? fifteenMin { get; set; }

    }

    public class XpDiffPerMinDeltasViewModel
    {
        public double? firstPartTime { get; set; }
        public double? secondPartTime { get; set; }

        public Tuple<double?,int> fiveMin { get; set; }
        public Tuple<double?, int> tenMin { get; set; }
        public Tuple<double?, int> fifteenMin { get; set; }
        
    }

    public class DamageTakenPerMinDeltasViewModel
    {
        public double? firstPartTime { get; set; }
        public double? secondPartTime { get; set; }
    }

    public class DamageTakenDiffPerMinDeltasViewModel
    {
        public double? firstPartTime { get; set; }
        public double? secondPartTime { get; set; }
    }

    public class DeathCountViewModel
    {
        public int? fiveMin { get; set; }
        public int? tenMin { get; set; }
        public int? fifteenMin { get; set; }
        public int? twentyMin { get; set; }
        public int? twentyFiveMin { get; set; }
        public int? thirtyMin { get; set; }
    }

    public class FrameViewModel
    {
        public List<EventViewModel> events { get; set; }
        public long timestamp { get; set; }
    }

    public class ParticipantFrameViewModel
    {
        public int participantId { get; set; }
        public int currentGold { get; set; }
        public int totalGold { get; set; }
        public int level { get; set; }
        public int xp { get; set; }
        public int minionsKilled { get; set; }
        public int jungleMinionsKilled { get; set; }

    }

    public class EventViewModel
    {
        public string type { get; set; }
        public long timestamp { get; set; }

        //properties for type "ward_placed"
        public string wardType { get; set; }
        public int creatorId { get; set; }

        //properties for type "ward_kill" "champion_kill"
        public int killerId { get; set; }

        //properties for type "Skill_level_up"
        public int skillSlot { get; set; }
        public string levelUpType { get; set; }

        //properties for type "Item_purchased"
        public int itemId { get; set; }
        public int participantId { get; set; }

        //properties for type "Champion_kill"
        public int victimId { get; set; }
        public int[] assistingParticipantIds { get; set; }

        public PositionViewModel position { get; set; }

        //properties for type "Elite_Monster_Kill"
        public string monsterType { get; set; }
        public string monsterSubType { get; set; }

        //properties for type "Building_kill"
        public string buildingType { get; set; }
        public string laneType { get; set; }
        public string towerType { get; set; }
        public int teamId { get; set; }

    }

    public class PositionViewModel
    {
        public int x { get; set; }
        public int y { get; set; }
    }

}