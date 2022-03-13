using Terrain.Models;
using UnityEngine;

namespace Terrain.Extensions
{
    internal static class WorldExtensions
    {
        public static bool TryGetBlock(this World world, Vector3Int position, out BlockType blockType)
        {
            var (x, y, z) = (position.x, position.y, position.z);
            return world.TryGetBlock(x, y, z, out blockType);
        }
    }
}
