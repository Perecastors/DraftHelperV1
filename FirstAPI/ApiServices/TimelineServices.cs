﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.ApiServices
{
    public class TimelineServices
    {
        public double? getCsDiffByTimingMark(List<Frame> frames, int participantId, int opponentParticipantId, int timingMark)
        {

            if (participantId == 0 || opponentParticipantId == 0)
                return null;

            double? result = null;
            var frame = GetFrameByTimingMark(frames, timingMark);
            var participantFrame = frame.participantFrames.Where(x => x.participantId == participantId).FirstOrDefault();
            var opposantParticipantFrame = frame.participantFrames.Where(x => x.participantId == opponentParticipantId).FirstOrDefault();

            var participantFarm = participantFrame.minionsKilled;
            var opposantParticipantFarm = opposantParticipantFrame.minionsKilled;

            result = participantFarm - opposantParticipantFarm;

            return result;
        }

        public Tuple<double?, int,int> getXpDiffByTimingMark(List<Frame> frames, int participantId, int opponentParticipantId, int timingMark)
        {
            if (participantId == 0 || opponentParticipantId == 0)
                return null;

            var frame = GetFrameByTimingMark(frames, timingMark);
            var participantFrame = frame.participantFrames.Where(x => x.participantId == participantId).FirstOrDefault();
            var opposantParticipantFrame = frame.participantFrames.Where(x => x.participantId == opponentParticipantId).FirstOrDefault();

            var participantXp = participantFrame.xp;
            var opposantParticipantXp = opposantParticipantFrame.xp;

            var participantLevel = participantFrame.level;
            var opposantParticipantLevel = opposantParticipantFrame.level;

            var tuple = new Tuple<double?, int,int>(participantXp - opposantParticipantXp, participantLevel , opposantParticipantLevel);

            return tuple;
        }

        private Frame GetFrameByTimingMark(List<Frame> frames, int timingMark)
        {
            Frame frame = new Frame();
            if (timingMark <= 16)
            {
                frame = frames.Skip(2).Where(x => x.timestamp.ToString().Substring(0, 3) == (timingMark * 60).ToString()).FirstOrDefault();
            }
            else if (timingMark > 16)
            {
                frame = frames.Skip(2).Where(x => x.timestamp.ToString().Substring(0, 4) == (timingMark * 60).ToString()).FirstOrDefault();
            }

            frame = frames.Where(x => Math.Floor((decimal)x.timestamp / 1000) == (timingMark * 60)).FirstOrDefault();

            return frame;
        }

        private List<Frame> GetListFrameByTimingMark(List<Frame> frames,int beginTimingMark, int endTimingMark)
        {
            List<Frame> lframes = new List<Frame>();

            lframes = frames.Where(x => Math.Floor((decimal)x.timestamp / 1000) > (beginTimingMark * 60) &&
                                    Math.Floor((decimal)x.timestamp / 1000) <= endTimingMark * 60
                                    ).ToList();

            if(beginTimingMark >= 20 && endTimingMark==0)
            {
                lframes = frames.Where(x => Math.Floor((decimal)x.timestamp / 1000) > (beginTimingMark * 60)).ToList();
            }

            return lframes;
        }

        public int? GetNbDeathBetweenTimingMark(List<Frame> frames, int participantId, int beginTimingMark, int endTimingMark)
        {
            int? nbDeath = null;
            List<List<Event>> levents = GetListFrameByTimingMark(frames, beginTimingMark, endTimingMark).Select(x => x.events).ToList();
            if (levents != null && levents.Count() > 0)
            {
                if (nbDeath == null)
                {
                    nbDeath = 0;
                }

                foreach (var events in levents)
                {
                    var count = events.Where(x => x.type.ToLower() == "champion_kill" && x.victimId == participantId).Count();
                    if (count > 0)
                    {
                        
                        nbDeath = nbDeath + count;
                    }
                }

            }
            return nbDeath;
        }

        public int? GetNbWardPutBetweenTimingMark(List<Frame> frames,int participantId,int beginTimingMark,int endTimingMark)
        {
            int? nbWardPut = null;
            List<List<Event>> levents = GetListFrameByTimingMark(frames, beginTimingMark, endTimingMark).Select(x => x.events).ToList();
            if (levents != null && levents.Count() > 0)
            {
                if (nbWardPut == null)
                {
                    nbWardPut = 0;
                }

                foreach (var events in levents)
                {
                    var count = events.Where(x => x.type.ToLower() == "ward_placed" && x.wardType.ToLower() != "control_ward" && x.creatorId == participantId).Count();
                    if (count > 0)
                    {

                        nbWardPut = nbWardPut + count;
                    }
                }

            }

            return nbWardPut;
        }

        public int? GetNbPinkPutBetweenTimingMark(List<Frame> frames, int participantId, int beginTimingMark, int endTimingMark)
        {
            int? nbWardPut = null;
            List<List<Event>> levents = GetListFrameByTimingMark(frames, beginTimingMark, endTimingMark).Select(x => x.events).ToList();
            if (levents != null && levents.Count() > 0)
            {
                if (nbWardPut == null)
                {
                    nbWardPut = 0;
                }

                foreach (var events in levents)
                {
                    var count = events.Where(x => x.type.ToLower() == "ward_placed" && x.wardType.ToLower() == "control_ward" && x.creatorId == participantId).Count();
                    if (count > 0)
                    {

                        nbWardPut = nbWardPut + count;
                    }
                }

            }

            return nbWardPut;
        }

        public int? GetNbWardDestroyedBetweenTimingMark(List<Frame> frames, int participantId, int beginTimingMark, int endTimingMark)
        {
            int? nbWard = null;
            List<List<Event>> levents = GetListFrameByTimingMark(frames, beginTimingMark, endTimingMark).Select(x => x.events).ToList();
            if (levents != null && levents.Count() > 0)
            {
                if (nbWard == null)
                {
                    nbWard = 0;
                }

                foreach (var events in levents)
                {
                    var count = events.Where(x => x.type.ToLower() == "ward_kill" && x.killerId == participantId).Count();
                    if (count > 0)
                    {

                        nbWard = nbWard + count;
                    }
                }

            }

            return nbWard;
        }

    }
}