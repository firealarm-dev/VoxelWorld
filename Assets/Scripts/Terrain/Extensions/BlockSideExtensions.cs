using System.Collections.Generic;
using Terrain.Game;
using Terrain.Models;
using UnityEngine;

namespace Terrain.Extensions
{
    internal static class BlockSideExtensions
    {
        private static readonly Dictionary<BlockSide, Vector3Int> SideDirections = new Dictionary<BlockSide, Vector3Int>
        {
            [BlockSide.Front] = Vector3Int.forward,
            [BlockSide.Back] = Vector3Int.back,
            [BlockSide.Left] = Vector3Int.left,
            [BlockSide.Right] = Vector3Int.right,
            [BlockSide.Top] = Vector3Int.up,
            [BlockSide.Bottom] = Vector3Int.down,
        };

        public static Vector3Int ToVector(this BlockSide blockSide)
        {
            return SideDirections[blockSide];
        }
    }
}
