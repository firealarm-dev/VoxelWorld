using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private int renderDistance = 3;
        [SerializeField] private bool randomSeed = true;
        [SerializeField] private Material material;
        [SerializeField] private NoiseOptions noiseOptions;

        private NoiseGenerator _noiseGenerator;
        private GameObject[] _players;

        private BlockSide[] _blockSides = (BlockSide[])Enum.GetValues(typeof(BlockSide));

        private List<Chunk> _chunksToRender = new List<Chunk>();
        private List<Chunk> _chunksToDestroy = new List<Chunk>();
        private Dictionary<Vector2Int, ChunkObject> _chunkObjects = new Dictionary<Vector2Int, ChunkObject>();

        private static readonly WorldTerrain WorldTerrain = new WorldTerrain(16, 56);

        private void Awake()
        {
            if (randomSeed)
            {
                noiseOptions.Seed = Random.Range(-100000, 100000);
            }

            _noiseGenerator = new NoiseGenerator(noiseOptions);
            _players = GameObject.FindGameObjectsWithTag("Player");
        }

        private void Update()
        {
            foreach (var player in _players)
            {
                var position = player.transform.position;

                var positionX = Mathf.FloorToInt(position.x / 16) * 16;
                var positionZ = Mathf.FloorToInt(position.z / 16) * 16;

                for (var x = positionX - 16 * renderDistance; x <= positionX + 16 * renderDistance; x += 16)
                {
                    for (var z = positionZ - 16 * renderDistance; z <= positionZ + 16 * renderDistance; z += 16)
                    {
                        var chunkPosition = new Vector2Int(x, z);

                        if (!WorldTerrain.TryGetChunk(x, z, out _) && _chunksToRender.All(c => c.Position != chunkPosition))
                        {
                            var chunk = CreateChunk(chunkPosition);
                            _chunksToRender.Add(chunk);
                        }
                    }
                }

                foreach (var chunk in WorldTerrain)
                {
                    if (Mathf.Abs(positionX - chunk.Position.x) > 16 * (renderDistance + 3) ||
                        Mathf.Abs(positionZ - chunk.Position.y) > 16 * (renderDistance + 3))
                    {
                        _chunksToDestroy.Add(chunk);
                    }
                }
            }

            foreach (var chunk in _chunksToRender)
            {
                var chunkObject = RenderChunk(chunk);
                _chunkObjects.Add(chunk.Position, chunkObject);
            }

            foreach (var chunk in _chunksToDestroy)
            {
                WorldTerrain.Remove(chunk.Position);

                if (_chunkObjects.TryGetValue(chunk.Position, out var chunkObject))
                {
                    _chunkObjects.Remove(chunk.Position);
                    Destroy(chunkObject);
                }
            }

            _chunksToRender.Clear();
            _chunksToDestroy.Clear();
        }

        private Chunk CreateChunk(Vector2Int position)
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

            return chunk;
        }

        private ChunkObject RenderChunk(Chunk chunk)
        {
            var chunkObject = new ChunkObject(chunk, transform, material);
            var meshBuilder = new VoxelMeshBuilder();
            
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

                foreach (var blockSide in _blockSides)
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

            return chunkObject;
        }
    }
}
