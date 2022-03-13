using System;
using UnityEngine;

namespace Terrain.Models
{
    [Serializable]
    internal class NoiseOptions
    {
        [field: SerializeField]
        public float Frequency { get; set; } = 1.0f;
        
        [field: SerializeField]
        public float Amplitude { get; set; } = 1.0f;

        [field: SerializeField]
        public float Persistence { get; set; } = 0.5f;

        [field: SerializeField]
        public float Smoothness { get; set; } = 0.01f;

        [field: SerializeField]
        public int Octaves { get; set; } = 4;
        
        [field: SerializeField]
        public int Seed { get; set; }
    }
}
