using System;

using BlocksHero.Tiles;

namespace BlocksHero.Core
{
    public class Node
    {
        public NodeType Type { get; private set; }
        public Guid Id { get; private set; }

        public TileGroup TileGroup { get; set; }

        public Node(NodeType type, Tile tile) : this(type, tile, Guid.NewGuid())
        {
        }

        public Node(NodeType type, Tile tile, Guid id)
        {
            Id = id;
            Type = type;
            TileGroup = new TileGroup
            {
                Tile = tile,
                TileShape = BlokusShape.RandomTileShape(type.Shape),
                Rotatable = true
            };
        }
    }
}
