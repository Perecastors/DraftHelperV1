using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstAPI.ApiServices
{
    public class TimelineServices
    {
        public List<Frame> GetFramesByParticipantId()
        {
            return null;
        }

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

        public Tuple<double?,int> getXpDiffByTimingMark(List<Frame> frames, int participantId, int opponentParticipantId, int timingMark)
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

            var tuple = new Tuple<double?, int>(participantXp-opposantParticipantXp,participantLevel-opposantParticipantLevel);

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
            return frame;
        }
    }
}