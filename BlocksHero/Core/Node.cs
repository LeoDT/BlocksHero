using System;

using Microsoft.Xna.Framework;

using BlocksHero.Tiles;

namespace BlocksHero.Core
{
    public class Node
    {
        public NodeType Type { get; private set; }
        public Guid Id { get; private set; }

        public TileGroup TileGroup { get; set; }

        public Node(NodeType type, Point tile) : this(type, tile, Guid.NewGuid())
        {
        }

        public Node(NodeType type, Point tile, Guid id)
        {
            Id = id;
            Type = type;
            TileGroup = new TileGroup
            (
                tile,
                BlokusShape.RandomTileShape(type.Shape),
                true
            );
        }
    }
}
