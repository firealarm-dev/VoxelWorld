using Terrain.Models;
using UnityEngine;

namespace Terrain.Extensions
{
    internal static class ChunkExtensions
    {
        public static bool TryGetBlock(this Chunk chunk, Vector3Int position, out BlockType blockType)
        {
            var (x, y, z) = (position.x, position.y, position.z);
            return chunk.TryGetBlock(x, y, z, out blockType);
        }
    }
}
