using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Domain
{
    public class AnimationClip
    {
        public Guid Id;
        public string Name;
        public int FrameRate;
        public int Length;

        public List<ParameterTimelineTrack> Tracks;
    }
}
