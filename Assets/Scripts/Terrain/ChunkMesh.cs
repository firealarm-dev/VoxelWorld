using System.Collections.Generic;
using Terrain.Noise;
using UnityEngine;

namespace Terrain
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshFilter))]
    internal class ChunkMesh : MonoBehaviour
    {
        [SerializeField] private Material _material;
        
        private MeshRenderer _meshRenderer;
        private MeshCollider _meshCollider;
        private MeshFilter _meshFilter;

        private Chunk _chunk = new Chunk(16, 56);

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshCollider = GetComponent<MeshCollider>();
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void Start()
        {
            var noise = new FastNoiseLite();

            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            noise.SetSeed(Random.Range(-10000, 1000));
            noise.SetFrequency(0.2f);
            noise.SetFractalLacunarity(0.02f);
            noise.SetFractalOctaves(6);

            for (var x = 0; x < _chunk.Size; x++)
            {
                for (var y = 0; y < _chunk.Height; y++)
                {
                    for (var z = 0; z < _chunk.Size; z++)
                    {
                        var height = noise.GetNoise(x, z);
                        var groundHeight = _chunk.Height * 0.5f + height;

                        _chunk[x, y, z] = y <= groundHeight
                            ? BlockType.Grass
                            : BlockType.Air;
                    }
                }
            }

            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var uv = new List<Vector2>();

            for (var x = 0; x < _chunk.Size; x++)
            {
                for (var y = 0; y < _chunk.Height; y++)
                {
                    for (var z = 0; z < _chunk.Size; z++)
                    {
                        if (_chunk[x, y, z] == BlockType.Air)
                        {
                            continue;
                        }

                        var position = new Vector3(x, y, z);
                        var verticesPosition = vertices.Count;
                        
                        foreach (var vertice in BlockMesh.Vertices)
                        {
                            vertices.Add(vertice + position);
                        }
                        
                        foreach (var triangle in BlockMesh.Triangles)
                        {
                            triangles.Add(triangle + verticesPosition);
                        }
                    }
                }
            }

            _meshFilter.mesh = new Mesh
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray()
            };

            _meshRenderer.sharedMaterial = _material;
        }
    }
}
