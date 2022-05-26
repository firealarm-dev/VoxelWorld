using UnityEngine;

namespace Terrain.Game
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshCollider))]
    internal class ChunkMesh : MonoBehaviour
    {
        private Material _material;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        
        public static ChunkMesh Attach(GameObject gameObject, Material material)
        {
            var chunkMesh = gameObject.AddComponent<ChunkMesh>();
            chunkMesh._material = material;
            return chunkMesh;
        }

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshCollider = GetComponent<MeshCollider>();
        }

        public void Render(Mesh mesh)
        {
            _meshRenderer.sharedMaterial = _material;
            _meshCollider.sharedMesh = mesh;
            _meshFilter.mesh = mesh;
        }
    }
}
