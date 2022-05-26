using Terrain.Models;
using UnityEngine;

namespace Terrain.Extensions
{
    internal static class WorldExtensions
    {
        public static bool TryGetBlock(this WorldTerrain terrain, Vector3Int position, out BlockType blockType)
        {
            var (x, y, z) = (position.x, position.y, position.z);
            return terrain.TryGetBlock(x, y, z, out blockType);
        }
    }
}
