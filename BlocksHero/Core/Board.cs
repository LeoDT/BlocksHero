using System;

using BlocksHero.Tiles;

namespace BlocksHero.Core
{
    public class Board
    {
        public BoardType Type { get; private set; }
        public Guid Id { get; private set; }

        public TileGroup TileGroup { get; set; }

        public Board(BoardType type, Tile tile) : this(type, tile, Guid.NewGuid())
        {
        }

        public Board(BoardType type, Tile tile, Guid id)
        {
            Id = id;
            Type = type;
            TileGroup = new TileGroup
            {
                Tile = tile,
                TileShape = new TileShape(Type.Width, Type.Height)
            };
        }
    }
}
