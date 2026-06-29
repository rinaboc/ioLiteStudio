using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Domain
{
    public class ParameterTimelineTrack
    {
        public Guid Id;
        public Guid ParameterId;
        public List<TimelineKeyframe> Keyframes;
    }
}
