using System;
using Terrain;
using Terrain.Extensions;
using Terrain.Game;
using Terrain.Models;
using UnityEngine;
using Random=UnityEngine.Random;

namespace Terrain.Services
{
    internal class TerrainRenderer : MonoBehaviour
    {
        [SerializeField] private byte sizeInChunks = 5;
        [SerializeField] private bool randomSeed = true;
        [SerializeField] private Material material;
        [SerializeField] private NoiseOptions noiseOptions;

        private NoiseGenerator _noiseGenerator;

        private static readonly WorldTerrain WorldTerrain = new WorldTerrain(16, 56);

        private void Awake()
        {
            if (randomSeed)
            {
                noiseOptions.Seed = Random.Range(-100000, 100000);
            }

            _noiseGenerator = new NoiseGenerator(noiseOptions);
        }

        private void Start()
        {
            for (var x = 0; x < sizeInChunks; x++)
            {
                for (var z = 0; z < sizeInChunks; z++)
                {
                    var position = new Vector2Int(x * WorldTerrain.ChunkSize, z * WorldTerrain.ChunkSize);
                    CreateChunk(position);
                }
            }

            foreach (var chunk in WorldTerrain)
            {
                RenderChunk(chunk);
            }
        }

        private void CreateChunk(Vector2Int position)
        {
            var chunk = new Chunk(16, 56, position);
            WorldTerrain[position] = chunk;

            for (var i = 0; i < chunk.Size * chunk.Height * chunk.Size; i++)
            {
                var x = i % chunk.Size;
                var y = i / chunk.Size % chunk.Height;
                var z = i / chunk.Size / chunk.Height;

                var noise = _noiseGenerator.GetNoise(x + position.x, z + position.y);
                var height = noise.Map(0, chunk.Height, 0, 1);

                chunk[x, y, z] = y <= height
                    ? BlockType.Grass
                    : BlockType.Air;
            }
        }

        private void RenderChunk(Chunk chunk)
        {
            var chunkObject = new ChunkObject(chunk, transform, material);

            var meshBuilder = new VoxelMeshBuilder();
            var blockSides = (BlockSide[])Enum.GetValues(typeof(BlockSide));
            for (var i = 0; i < chunk.Size * chunk.Height * chunk.Size; i++)
            {
                var x = i % chunk.Size;
                var y = i / chunk.Size % chunk.Height;
                var z = i / chunk.Size / chunk.Height;

                var worldPosition = new Vector3Int(x + chunk.Position.x, y, z + chunk.Position.y);

                if (!WorldTerrain.TryGetBlock(worldPosition, out _))
                {
                    continue;
                }

                var localPosition = new Vector3Int(x, y, z);

                foreach (var blockSide in blockSides)
                {
                    var neightbourPosition = worldPosition + blockSide.ToVector();

                    if (!WorldTerrain.TryGetBlock(neightbourPosition, out var blockType) && blockType == BlockType.Air)
                    {
                        meshBuilder.AppendQuad(blockSide, localPosition);
                    }
                }
            }

            var mesh = meshBuilder.Build();
            chunkObject.Mesh.Render(mesh);
        }
    }
}
