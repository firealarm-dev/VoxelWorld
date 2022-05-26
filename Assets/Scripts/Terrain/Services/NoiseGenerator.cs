using Terrain.Abstractions;
using Terrain.Models;
using UnityEngine;

namespace Terrain.Services
{
    internal class NoiseGenerator : INoiseGenerator
    {
        private readonly NoiseOptions _options;

        public NoiseGenerator(NoiseOptions options)
        {
            _options = options;
        }
        
        public float GetNoise(float x, float y)
        {
            x *= _options.Smoothness;
            y *= _options.Smoothness;
            
            var value = 0.0f;
            var maxValue = 0.0f;

            var frequency = _options.Frequency;
            var amplitude = _options.Amplitude;

            for (var i = 0; i < _options.Octaves; i++)
            {
                var sampleX = (x + _options.Seed) * frequency;
                var sampleY = (y + _options.Seed) * frequency;
                
                value += Mathf.PerlinNoise(sampleX, sampleY) * amplitude;
                maxValue += amplitude;
                
                amplitude *= _options.Persistence;
                frequency *= 2;
            }

            return value / maxValue;
        }
    }
}
