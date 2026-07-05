using System;
using Core.Features;
using UnityEngine;

namespace Core.Domain
{
    public enum BlendMode { Absolute, Additive }

    public class Parameter
    {
        public Guid Id;
        public string Name;
        public int MaxValueInt;
        public int MinValueInt;
        public int CurrentValueInt;
        static readonly float PRECISION = 1000f;

        public BlendMode BlendMode;

        public float CurrentValueFloat
        {
            get => CurrentValueInt / PRECISION;
            set => CurrentValueInt = Mathf.RoundToInt(value * PRECISION);
        }

        public float MinValueFloat
        {
            get => MinValueInt / PRECISION;
            set => MinValueInt = Mathf.RoundToInt(value * PRECISION);
        }

        public float MaxValueFloat
        {
            get => MaxValueInt / PRECISION;
            set => MaxValueInt = Mathf.RoundToInt(value * PRECISION);
        }

        public Parameter() { }

        public Parameter(ParameterConfig config)
        {
            Name = config.Name;
            MinValueFloat = config.Min;
            MaxValueFloat = config.Max;
            BlendMode = config.BlendMode;
        }
    }
}
