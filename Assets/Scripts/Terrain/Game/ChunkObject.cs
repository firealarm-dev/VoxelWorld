using Terrain.Models;
using UnityEngine;

namespace Terrain.Game
{
    internal class ChunkObject
    {
        private readonly Chunk _chunk;
        private readonly GameObject _gameObject;

        public ChunkObject(Chunk chunk, Transform parent, Material material)
        {
            _chunk = chunk;
            _gameObject = new GameObject($"Chunk {chunk.Position}");

            _gameObject.transform.position = new Vector3Int(chunk.Position.x, 0, chunk.Position.y);
            _gameObject.transform.parent = parent;

            Mesh = ChunkMesh.Attach(_gameObject, material);
        }

        public ChunkMesh Mesh { get; }

        public void SetActive(bool value)
        {
            _gameObject.SetActive(value);
        }
        
        public static implicit operator Chunk(ChunkObject chunkObject)
        {
            return chunkObject._chunk;
        }

        public static implicit operator GameObject(ChunkObject chunkObject)
        {
            return chunkObject._gameObject;
        }
    }
}
