using System;
using Terrain.Extensions;
using Terrain.Game;
using Terrain.Models;
using UnityEngine;
using Random=UnityEngine.Random;

namespace Terrain.Services
{
    internal class TerrainRenderer : MonoBehaviour
    {
        [SerializeField] private byte _sizeInChunks = 5;
        [SerializeField] private bool _randomSeed = true;
        [SerializeField] private Material _material;
        [SerializeField] private NoiseOptions _noiseOptions;

        private NoiseGenerator _noiseGenerator;

        private static readonly World World = new World(16, 56);

        private void Awake()
        {
            if (_randomSeed)
            {
                _noiseOptions.Seed = Random.Range(-100000, 100000);
            }

            _noiseGenerator = new NoiseGenerator(_noiseOptions);
        }

        private void Start()
        {
            for (var x = 0; x < _sizeInChunks; x++)
            {
                for (var z = 0; z < _sizeInChunks; z++)
                {
                    var position = new Vector2Int(x * World.ChunkSize, z * World.ChunkSize);
                    CreateChunk(position);
                }
            }

            foreach (var chunk in World)
            {
                RenderChunk(chunk);
            }
        }

        private void CreateChunk(Vector2Int position)
        {
            var chunk = new Chunk(16, 56, position);
            World[position] = chunk;

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
            var chunkObject = new ChunkObject(chunk, transform, _material);

            var meshBuilder = new VoxelMeshBuilder();
            var blockSides = (BlockSide[])Enum.GetValues(typeof(BlockSide));
            for (var i = 0; i < chunk.Size * chunk.Height * chunk.Size; i++)
            {
                var x = i % chunk.Size;
                var y = i / chunk.Size % chunk.Height;
                var z = i / chunk.Size / chunk.Height;

                var worldPosition = new Vector3Int(x + chunk.Position.x, y, z + chunk.Position.y);

                if (!World.TryGetBlock(worldPosition, out _))
                {
                    continue;
                }

                var localPosition = new Vector3Int(x, y, z);

                foreach (var blockSide in blockSides)
                {
                    var neightbourPosition = worldPosition + blockSide.ToVector();

                    if (!World.TryGetBlock(neightbourPosition, out var blockType) && blockType == BlockType.Air)
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
