using UnityEngine;

namespace Terrain.Models
{
    public class Chunk
    {
        private readonly BlockType[] _blocks;

        public Chunk(ushort size, ushort height, Vector2Int position)
        {
            Size = size;
            Height = height;
            Position = position;

            _blocks = new BlockType[size * height * size];
        }

        public ushort Size { get; }

        public ushort Height { get; }

        public Vector2Int Position { get; }

        public BlockType this[int x, int y, int z]
        {
            get => _blocks[x * Size * Height + y * Size + z];
            set => _blocks[x * Size * Height + y * Size + z] = value;
        }

        public bool TryGetBlock(int x, int y, int z, out BlockType blockType)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Height || z < 0 || z >= Size)
            {
                blockType = BlockType.Nothing;
                return false;
            }
            
            blockType = this[x, y, z];
            return blockType != BlockType.Air;
        }
    }
}
