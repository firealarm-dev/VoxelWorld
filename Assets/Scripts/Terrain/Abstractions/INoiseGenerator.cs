namespace Terrain.Abstractions
{
    internal interface INoiseGenerator
    {
        float GetNoise(float x, float y);
    }
}
