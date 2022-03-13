using Terrain.Models;
using UnityEngine;

namespace Terrain.Abstractions
{
    internal interface IVoxelMeshBuilder
    {
        void AppendQuad(BlockSide side, Vector3Int position);

        void AppendQuad(BlockSide side, int x, int y, int z);

        Mesh Build();
    }
}
