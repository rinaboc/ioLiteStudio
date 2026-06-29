using System;
using UnityEngine;

namespace Core.Domain
{
    public enum InterpolationMode { Linear, Smooth }

    public class TimelineKeyframe
    {
        public Guid Id;
        public int Frame;
        public int ParameterValue;
        public InterpolationMode InterpolationMode;
    }
}
