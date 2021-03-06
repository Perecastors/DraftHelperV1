﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Web;

namespace FirstAPI.ApiServices
{

    public class Match
    {
        public string platformId { get; set; }
        public long gameId { get; set; }
        [JsonProperty("champion")]
        public int championId { get; set; }
        public int queue { get; set; }
        public int season { get; set; }
        public long timestamp { get; set; }
        public string role { get; set; }
        public string lane { get; set; }
    }

    public class MatchInfos
    {
        public long gameId { get; set; }
        public long gameCreation { get; set; }
        public int gameDuration { get; set; }
        public string gameVersion { get; set; }
        public List<Team> teams { get; set; }
        public List<Participant> participants { get; set; }
        public List<ParticipantIdentity> participantIdentities { get; set; }
    }

    public class Team
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
        public List<Ban> bans { get; set; }
    }

    public class Ban
    {
        public int championId { get; set; }
        public int pickTurn { get; set; }
    }

    public class Participant
    {
        public int participantId { get; set; }
        public int teamId { get; set; }
        public int championId { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public Stats stats { get; set; }
        public Timeline timeline { get; set; }
    }

    public class ParticipantIdentity
    {
        public int participantId { get; set; }
        public Player player { get; set; }
        public class Player
        {
            public string accountId { get; set; }
            public string summonerName { get; set; }
        }
    }

    public class Stats
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

    public class Timeline
    {
        public int participantId { get; set; }
        public string role { get; set; }
        public string lane { get; set; }
        public CreepsPerMinDeltas creepsPerMinDeltas { get; set; }
        public XpDiffPerMinDeltas xpDiffPerMinDeltas { get; set; }
        public GoldPerMinDeltas goldPerMinDeltas { get; set; }
        public CsDiffPerMinDeltas csDiffPerMinDeltas { get; set; }
        public DamageTakenDiffPerMinDeltas damageTakenDiffPerMinDeltas { get; set; }
        public DamageTakenPerMinDeltas damageTakenPerMinDeltas { get; set; }
        public XpPerMinDeltas XpPerMinDeltas { get; set; }

    }

    public class CreepsPerMinDeltas
    {
        [JsonProperty("0-10")]
        public double firstPartTime { get; set; }
        [JsonProperty("10-20")]
        public double secondPartTime { get; set; }
    }

    public class XpPerMinDeltas
    {
        [JsonProperty("0-10")]
        public double firstPartTime { get; set; }
        [JsonProperty("10-20")]
        public double secondPartTime { get; set; }
    }

    public class GoldPerMinDeltas
    {
        [JsonProperty("0-10")]
        public double firstPartTime { get; set; }
        [JsonProperty("10-20")]
        public double secondPartTime { get; set; }
    }

    public class CsDiffPerMinDeltas
    {
        [JsonProperty("0-10")]
        public double firstPartTime { get; set; }
        [JsonProperty("10-20")]
        public double secondPartTime { get; set; }
    }

    public class XpDiffPerMinDeltas
    {
        [JsonProperty("0-10")]
        public double firstPartTime { get; set; }
        [JsonProperty("10-20")]
        public double secondPartTime { get; set; }
    }

    public class DamageTakenPerMinDeltas
    {
        [JsonProperty("0-10")]
        public double firstPartTime { get; set; }
        [JsonProperty("10-20")]
        public double secondPartTime { get; set; }
    }

    public class DamageTakenDiffPerMinDeltas
    {
        [JsonProperty("0-10")]
        public double firstPartTime { get; set; }
        [JsonProperty("10-20")]
        public double secondPartTime { get; set; }
    }

    public class Frame
    {
        public List<ParticipantFrame> participantFrames { get; set; } //=> dans le json : c'est pas une liste de participantframe: il faudra modifier le json pour que ca marche bien
        public List<Event> events { get; set; }
        public long timestamp { get; set; }
    }

    public class FrameJson
    {
        public PartcipantFrameJson participantFrames { get; set; } //=> dans le json : c'est pas une liste de participantframe: il faudra modifier le json pour que ca marche bien
        public List<Event> events { get; set; }
        public long timestamp { get; set; }
    }

    public class PartcipantFrameJson
    {
        [JsonProperty("1")]
        public ParticipantFrame participantFrame1 { get; set; }
        [JsonProperty("2")]
        public ParticipantFrame participantFrame2 { get; set; }
        [JsonProperty("3")]
        public ParticipantFrame participantFrame3 { get; set; }
        [JsonProperty("4")]
        public ParticipantFrame participantFrame4 { get; set; }
        [JsonProperty("5")]
        public ParticipantFrame participantFrame5 { get; set; }
        [JsonProperty("6")]
        public ParticipantFrame participantFrame6 { get; set; }
        [JsonProperty("7")]
        public ParticipantFrame participantFrame7 { get; set; }
        [JsonProperty("8")]
        public ParticipantFrame participantFrame8 { get; set; }
        [JsonProperty("9")]
        public ParticipantFrame participantFrame9 { get; set; }
        [JsonProperty("10")]
        public ParticipantFrame participantFrame10 { get; set; }
    }

    public class ParticipantFrame
    {
        public int participantId { get; set; }
        public int currentGold { get; set; }
        public int totalGold { get; set; }
        public int level { get; set; }
        public int xp { get; set; }
        public int minionsKilled { get; set; }
        public int jungleMinionsKilled { get; set; }

    }

    public class Event
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

        public Position position { get; set; }

        //properties for type "Elite_Monster_Kill"
        public string monsterType { get; set; }
        public string monsterSubType { get; set; }

        //properties for type "Building_kill"
        public string buildingType { get; set; }
        public string laneType { get; set; }
        public string towerType { get; set; }
        public int teamId { get; set; }

    }

    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Summoner
    {
        [JsonProperty("name")]
        public string summonerName { get; set; }
    }
}