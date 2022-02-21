using UnityEngine;

namespace Terrain
{
    public class Chunk
    {
        private readonly BlockType[] _blocks;

        public Chunk(ushort size, ushort height)
        {
            Size = size;
            Height = height;
            
            _blocks = new BlockType[size * height * size];
        }
        
        public ushort Size { get; }

        public ushort Height { get; }

        public BlockType this[int x, int y, int z]
        {
            get => _blocks[x * Size * Height + y * Size + z];
            set => _blocks[x * Size * Height + y * Size + z] = value;
        }
    }
}
