using UnityEngine;

namespace Terrain.Game
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    internal class ChunkMesh : MonoBehaviour
    {
        private Material _material;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        
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
        }

        public void Render(Mesh mesh)
        {
            _meshRenderer.sharedMaterial = _material;
            _meshFilter.mesh = mesh;
        }
    }
}
