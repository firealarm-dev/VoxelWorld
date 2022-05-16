using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Terrain.Models;
using UnityEngine;

namespace Terrain
{
    internal class WorldTerrain : IEnumerable<Chunk>
    {
        private readonly Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();

        public WorldTerrain(ushort chunkSize, ushort chunkHeight)
        {
            ChunkSize = chunkSize;
            ChunkHeight = chunkHeight;
        }
        
        public ushort ChunkSize { get; }
        
        public ushort ChunkHeight { get; }

        public bool TryGetChunk(int x, int z, [CanBeNull] out Chunk chunk)
        {
            var worldX = Mathf.FloorToInt((float)x / ChunkSize) * ChunkSize;
            var worldZ = Mathf.FloorToInt((float)z / ChunkSize) * ChunkSize;

            var chunkPosition = new Vector2Int(worldX, worldZ);
            return _chunks.TryGetValue(chunkPosition, out chunk);
        }

        public bool TryGetBlock(int x, int y, int z, out BlockType blockType)
        {
            if (!TryGetChunk(x, z, out var chunk))
            {
                blockType = BlockType.Nothing;
                return false;
            }

            var chunkX = x % ChunkSize;
            var chunkY = y % ChunkHeight;
            var chunkZ = z % ChunkSize;

            return chunk!.TryGetBlock(chunkX, chunkY, chunkZ, out blockType);
        }

        public Chunk this[Vector2Int position]
        {
            get => _chunks[position];
            set => _chunks[position] = value;
        }
        
        public IEnumerator<Chunk> GetEnumerator()
        {
            return _chunks.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
