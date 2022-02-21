using UnityEngine;

namespace Terrain
{
    internal class BlockMesh : MonoBehaviour
    {
        public static Vector3[] Vertices { get; } = new Vector3[8] 
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(0.0f, 1.0f, 1.0f),
        };

        public static int[,] Triangles { get; } = new int[6,6] 
        {
            { 0, 3, 1, 1, 3, 2 }, // Back Face
            { 5, 6, 4, 4, 6, 7 }, // Front Face
            { 3, 7, 2, 2, 7, 6 }, // Top Face
            { 1, 5, 0, 0, 5, 4 }, // Bottom Face
            { 4, 7, 0, 0, 7, 3 }, // Left Face
            { 1, 2, 5, 5, 2, 6 }, // Right Face
        };

        public Vector2[] UVs { get; } = new Vector2[6] 
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
        };
    }
}
