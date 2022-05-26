using System.Collections.Generic;
using Terrain.Abstractions;
using Terrain.Models;
using UnityEngine;

namespace Terrain.Services
{
    internal class VoxelMeshBuilder : IVoxelMeshBuilder
    {
        private readonly List<Vector3> _vertices = new List<Vector3>();
        private readonly List<int> _triangles = new List<int>();

        public void AppendQuad(BlockSide side, Vector3Int position)
        {
            var (x, y, z) = (position.x, position.y, position.z);
            AppendQuad(side, x, y, z);
        }
        
        public void AppendQuad(BlockSide side, int x, int y, int z)
        {           
            _triangles.Add(0 + _vertices.Count);
            _triangles.Add(1 + _vertices.Count);
            _triangles.Add(2 + _vertices.Count);
                
            _triangles.Add(0 + _vertices.Count);
            _triangles.Add(2 + _vertices.Count);
            _triangles.Add(3 + _vertices.Count);
            
            switch (side)
            {
                case BlockSide.Front:
                    _vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                    _vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                    _vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                    _vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                    break;
                
                case BlockSide.Back:
                    _vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                    _vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                    _vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                    _vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                    break;
                
                case BlockSide.Left:
                    _vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                    _vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                    _vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                    _vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                    break;
                
                case BlockSide.Right:
                    _vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                    _vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                    _vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                    _vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                    break;
                
                case BlockSide.Top:
                    _vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                    _vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                    _vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                    _vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                    break;
                
                case BlockSide.Bottom:
                    _vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                    _vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                    _vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                    _vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                    break;
            }
        }

        public Mesh Build()
        { var mesh = new Mesh
            {
                vertices = _vertices.ToArray(),
                triangles = _triangles.ToArray()
            };

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            
            return mesh;
        }
    }
}
